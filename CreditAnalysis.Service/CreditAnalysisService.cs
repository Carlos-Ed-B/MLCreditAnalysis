using CreditAnalysis.Model;
using CreditAnalysis.Service.Interfaces;
using IBM.Cloud.SDK.Core.Http;
using Infrastructure.Layer.Base;
using Infrastructure.Layer.Environments;
using Infrastructure.Layer.Extensions;
using Infrastructure.Layer.Helpers;
using ML.Services.Azure.Interfaces;
using ML.Services.Azure.Models;
using ML.Services.Enums;
using ML.Services.Helpers;
using ML.Services.IBM;
using ML.Services.IBM.Helpers;
using ML.Services.IBM.Model;
using ML.Services.IBM.Model.Enums;
using ML.Services.Models;
using ML.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CreditAnalysis.Service
{
    public class CreditAnalysisService : BaseService, ICreditAnalysisService
    {
        private readonly IIBMVisualRecognitionService _ibmVisualRecognitionService;
        private readonly IAzureVisualRecognitionService _azureVisualRecognitionService;
        private readonly ICreditAnalysisMLService _creditAnalysisMLService;
        public CreditAnalysisService(IIBMVisualRecognitionService ibmVisualRecognitionService, IAzureVisualRecognitionService azureVisualRecognitionService, ICreditAnalysisMLService creditAnalysisMLService)
        {
            this._ibmVisualRecognitionService = ibmVisualRecognitionService;
            this._azureVisualRecognitionService = azureVisualRecognitionService;
            this._creditAnalysisMLService = creditAnalysisMLService;
        }

        /// <summary>
        /// Responsavel por fazer a analise dos dados do cliente.
        /// </summary>
        public async Task<bool> DoCreditAnalysisAsync(ClientCreditAnalysisModel clientCreditAnalysisModel)
        {
            /// Validamos se um modelo foi informado pelo cliente, 
            if (clientCreditAnalysisModel.ModelType.ToInteger() == 0)
            {
                /// Em caso de negativa, informamos ao cliente que o modelo nao foi informado
                this.AddError("Modelo não selecionado");
                return false;
            }

            /// Preenche os dados do upload
            this.FillFileByte(clientCreditAnalysisModel);

            /// Gero um ID para o cliente com base na data e hora do envio
            clientCreditAnalysisModel.Id = DateTime.Now.ToFileName();

            /// Faz o log dos dados de entrada para possivel analse posterior em casos de erro ou melhora do modelo
            this.DoLogOnBegin(clientCreditAnalysisModel);

            /// COnverto os dados do request do frontEnd para ToCreditAnalysisMLModel
            var creditAnalysisMLModel = this.ToCreditAnalysisMLModel(clientCreditAnalysisModel);

            /// Crio o objeto de container de resultado da analise de credito
            var classifyPersonalResultModel = new ClassifyPersonalResultModel() { };

            /// Faco a analise de risco de credito, e pego o score
            classifyPersonalResultModel.CreditAnalysisScore = await this._creditAnalysisMLService.ClassifyAsync(creditAnalysisMLModel);
            /// Crio uma faixa de risco do score
            classifyPersonalResultModel.CreditAnalysisScoreRisk = AnalysisHelper.GetScoreRisk(classifyPersonalResultModel.CreditAnalysisScore);

            /// Faco a analise de nivel de não pagamento, caso tenho um indice alto de nao pagamento, nego o pedido do cliente
            if (classifyPersonalResultModel.CreditAnalysisScoreRisk == ScoreRiskEnum.High || classifyPersonalResultModel.CreditAnalysisScoreRisk == ScoreRiskEnum.VeryHigh)
            {
                /// Adiciono uma mensagem ao usuario e para o processamento 
                this.AddError("Analise de pagamento em dia baixo.");
                clientCreditAnalysisModel.MessageError = this.GetFirstError();
                return false;
            }

            /// Faco uma analise da self do cliente, onde analise se existe uma pessoa
            classifyPersonalResultModel.ClassifyPerson = this.ClassifyPerson(clientCreditAnalysisModel);
            /// Faco uma analise da self do cliente, onde analise se existe conteudo explicito
            classifyPersonalResultModel.ClassifyExplicitSex = this.ClassifyExplicitSex(clientCreditAnalysisModel);

            /// Apos a analise da foto, verificamos se ela e valida
            if (!this.IsValid())
            {
                /// Caso nao seja valido, retorno a primeria mensagem de erro ao usuario e para o processamento
                clientCreditAnalysisModel.MessageError = this.GetFirstError();
                return this.IsValid();
            }

            /// Continuamos com a analise
            /// Fazermos a analise dos dados informados pelo usuario em relacao a self informada por ele
            classifyPersonalResultModel.VisionFaceResul = await this.ClassifyPersonalDataAsync(clientCreditAnalysisModel);

            /// apos a analise,salvo os dados processados por ela
            classifyPersonalResultModel.Status = this.IsValid();
            classifyPersonalResultModel.MessageError = this.GetFirstError();

            ///Caso o processamento de imagem tenha retornado dados
            if (classifyPersonalResultModel.VisionFaceResul.Any())
            {
                /// Pego a idade e genero do processamento
                clientCreditAnalysisModel.VisionFaceAge = classifyPersonalResultModel.VisionFaceResul.FirstOrDefault().FaceAttributes.Age;
                clientCreditAnalysisModel.VisionFaceGender = classifyPersonalResultModel.VisionFaceResul.FirstOrDefault().FaceAttributes.Gender;
            }

            /// Faco o log do pos processamento
            this.DoLogOnEnd(clientCreditAnalysisModel, classifyPersonalResultModel);

            /// retorno o status do processamento
            return this.IsValid();
        }

        /// <summary>
        /// Faz a conversao do objeto para ToCreditAnalysisMLModel
        /// </summary>
        private CreditAnalysisMLModel ToCreditAnalysisMLModel(ClientCreditAnalysisModel clientCreditAnalysisModel)
        {
            return new CreditAnalysisMLModel()
            {
                Casapropria = clientCreditAnalysisModel.OwnHome ? 1 : 0,
                Escolaridade = clientCreditAnalysisModel.Schooling,
                Estadocivil = clientCreditAnalysisModel.MaritalStatus,
                Etnia = clientCreditAnalysisModel.Ethnicity,
                Idade = clientCreditAnalysisModel.Age,
                Nome = clientCreditAnalysisModel.Name,
                Outrasrendas = clientCreditAnalysisModel.ExtraSalary ? 1 : 0,
                Sexo = clientCreditAnalysisModel.Gender == Model.Enums.GenderEnum.Male ? 0 : 1,
                Renda = clientCreditAnalysisModel.Salary,
                ModelType = (CreditAnalysisModelTypeEnum)clientCreditAnalysisModel.ModelType.ToInteger()
            };
        }

        /// <summary>
        /// Preenche os dados do upload, atualmente temos 2 formas: upload e base64
        /// </summary>
        /// <param name="clientCreditAnalysisModel"></param>
        private void FillFileByte(ClientCreditAnalysisModel clientCreditAnalysisModel)
        {
            /// Verifico se o arquivo de upload foi informado
            if (clientCreditAnalysisModel.FileUploadByte == null && clientCreditAnalysisModel.FileUpload != null)
            {
                /// Caso tenha sido informado, converto uploado para byte
                clientCreditAnalysisModel.FileUploadByte = clientCreditAnalysisModel.FileUpload.ToFileBytes();
                return;
            }

            /// Verifico se o arquivo base64 foi informado
            if (!string.IsNullOrEmpty(clientCreditAnalysisModel.ImageFileUploadBase64))
            {
                /// Pego as informacoes do arquivo Base64
                var base64 = clientCreditAnalysisModel.ImageFileUploadBase64;

                /// O sistema pode aceistar um base64 de 2 formas, um apenas com o base64 e outro com todo o html, por isso, fazemos esse tratamento
                if (base64.Contains(","))
                {
                    ///Caso for um base64 com dados do html, consideramos apenas o base64
                    base64 = base64.Split(',')[1];
                }

                /// Atualizamos as informacoes do base64 tratado
                clientCreditAnalysisModel.ImageFileUploadBase64 = base64;

                /// Convertemos o base64 para byte
                clientCreditAnalysisModel.FileUploadByte = Helper.ImageBase64ToByte(clientCreditAnalysisModel.ImageFileUploadBase64);
            }
        }

        /// <summary>
        /// Verifique se na imagem possui uma pessoa
        /// </summary>
        /// <param name="clientCreditAnalysisModel"></param>
        /// <returns></returns>
        private DetailedResponse<ClassifiedImages> ClassifyPerson(ClientCreditAnalysisModel clientCreditAnalysisModel)
        {
            var classifyPerson = this._ibmVisualRecognitionService.Classify(clientCreditAnalysisModel.FileUploadByte, IBMImageClassifierEnum.Default);

            var classifierClassPersonResult = classifyPerson.GetClassifierClassResult(IBMImageClassifierEnum.Person);

            if (classifierClassPersonResult == null || classifierClassPersonResult.Score == null)
            {
                this.AddError("Não foi possivel classificar como pessoa.");
                return classifyPerson;
            }

            var scoreRiskPerson = AnalysisHelper.GetScoreRisk(classifierClassPersonResult.Score.Value);

            if (scoreRiskPerson == ScoreRiskEnum.VeryLow || scoreRiskPerson == ScoreRiskEnum.Low)
            {
                this.AddError("Não foi possivel identificar a pessoa na imagem.");
                return classifyPerson;
            }

            return classifyPerson;
        }

        /// <summary>
        /// Verifique se a imagem apresenta conteúdo explícito
        /// </summary>
        /// <param name="clientCreditAnalysisModel"></param>
        /// <returns></returns>
        private DetailedResponse<ClassifiedImages> ClassifyExplicitSex(ClientCreditAnalysisModel clientCreditAnalysisModel)
        {
            var classifyPerson = this._ibmVisualRecognitionService.Classify(clientCreditAnalysisModel.FileUploadByte, IBMImageClassifierEnum.ExplicitSex);

            var classifierClassPersonResult = classifyPerson.GetClassifierClassResult(IBMImageClassifierEnum.ExplicitSex);

            ////Quando ele nao encontra, verifico a inversa
            if (classifierClassPersonResult == null || classifierClassPersonResult.Score == null)
            {
                this.ClassifyNotExplicitSex(classifyPerson);
                return classifyPerson;
            }

            var scoreRiskPerson = AnalysisHelper.GetScoreRisk(classifierClassPersonResult.Score.Value);

            if (scoreRiskPerson == ScoreRiskEnum.VeryHigh || scoreRiskPerson == ScoreRiskEnum.High)
            {
                this.AddError("Possivel conteúdo explícito na imagem.");
            }

            return classifyPerson;
        }

        /// <summary>
        /// Verifique se a imagem apresenta conteúdo não explícito
        /// </summary>
        /// <param name="classifyResult"></param>
        /// <returns></returns>
        private void ClassifyNotExplicitSex(DetailedResponse<ClassifiedImages> classifyResult)
        {
            var classifierClassPersonResult = classifyResult.GetClassifierClassResult(IBMImageClassifierEnum.NotExplicitSex);

            if (classifierClassPersonResult == null || classifierClassPersonResult.Score == null)
            {
                this.AddError("Não foi possivel classificar como conteúdo não explícito.");
            }

            var scoreRiskPerson = AnalysisHelper.GetScoreRisk(classifierClassPersonResult.Score.Value);

            if (scoreRiskPerson == ScoreRiskEnum.VeryLow || scoreRiskPerson == ScoreRiskEnum.Low)
            {
                this.AddError("Possivel conteúdo explícito na imagem.");
            }
        }

        public async Task<List<VisionFaceResultModel>> ClassifyPersonalDataAsync(ClientCreditAnalysisModel clientCreditAnalysisModel, bool saveFaceLandmarksPointOnImage = true)
        {
            var personalDataList = await this._azureVisualRecognitionService.Classify(clientCreditAnalysisModel.FileUploadByte);

            if (personalDataList.Count == 0)
            {
                this.AddError("Não foi encontrada nenhuma pessoa na imagem, por favor envie uma foto sua.");
                return personalDataList;
            }

            if (personalDataList.Count > 1)
            {
                this.AddError("Foi encontrada mais de uma pessoa na imagem, por favor envie apenas a sua foto.");
                return personalDataList;
            }

            var personalData = personalDataList.FirstOrDefault();

            if (!personalData.FaceAttributes.Gender.ToLower().Equals(clientCreditAnalysisModel.Gender.ToDescription().ToLower()))
            {
                this.AddError("Genero incompativel com o informado no cadastro.");
                return personalDataList;
            }

            if (personalData.FaceAttributes.Age > clientCreditAnalysisModel.Age + 5)
            {
                this.AddError("Idade superior incompativel com o informado no cadastro.");
                return personalDataList;
            }

            if (personalData.FaceAttributes.Age < clientCreditAnalysisModel.Age - 5)
            {
                this.AddError("Idade inferior incompativel com o informado no cadastro.");
                return personalDataList;
            }

            if (saveFaceLandmarksPointOnImage)
            {
                clientCreditAnalysisModel.ImageFile = this._azureVisualRecognitionService.GetFaceLandmarksPointOnImage(personalData, clientCreditAnalysisModel.FileUploadByte, "");
                clientCreditAnalysisModel.ImageFileBase64 = Convert.ToBase64String(clientCreditAnalysisModel.ImageFile);
            }

            base.MessageSuccess = "Crédito aprovado";

            return personalDataList;
        }

        public void DoLogOnBegin(ClientCreditAnalysisModel clientCreditAnalysisModel)
        {
            var pathLog = this.GetLogPath(clientCreditAnalysisModel);
            Directory.CreateDirectory(pathLog);

            var filePath = this.GetLogPathFile(clientCreditAnalysisModel, "ClientCreditAnalysisModel01Begin.json");

            JsonHelper.SerializeToFile(filePath, clientCreditAnalysisModel, true, true);

            var fileName = this.GetImageFileName(clientCreditAnalysisModel);

            var filePathImage = this.GetLogPathFile(clientCreditAnalysisModel, fileName);

            Helper.SaveFile(filePathImage, clientCreditAnalysisModel.FileUploadByte);
        }

        public void DoLogOnEnd(ClientCreditAnalysisModel clientCreditAnalysisModel, ClassifyPersonalResultModel classifyPersonalResultModel)
        {
            var filePath = this.GetLogPathFile(clientCreditAnalysisModel, "ClientCreditAnalysisModel03End.json");
            JsonHelper.SerializeToFile(filePath, clientCreditAnalysisModel, true, true);

            var fileclassifyPersonalResultModelPath = this.GetLogPathFile(clientCreditAnalysisModel, "ClassifyPersonalResultModel.json");
            JsonHelper.SerializeToFile(fileclassifyPersonalResultModelPath, classifyPersonalResultModel, true, true);

            if (clientCreditAnalysisModel.ImageFile != null)
            {
                var fileName = this.GetImageFileName(clientCreditAnalysisModel, "FaceLandmarksPoint.jpg");
                var filePathImage = this.GetLogPathFile(clientCreditAnalysisModel, fileName);
                Helper.SaveFile(filePathImage, clientCreditAnalysisModel.ImageFile);
            }
        }

        private string GetImageFileName(ClientCreditAnalysisModel clientCreditAnalysisModel, string fileName = "imageSend.jpg")
        {
            if (clientCreditAnalysisModel.FileUpload != null)
            {
                fileName = clientCreditAnalysisModel.FileUpload.FileName;
            }

            return fileName;
        }

        private string GetLogPath(ClientCreditAnalysisModel clientCreditAnalysisModel)
        {
            return @$"{InfrastructureEnvironment.ContentRoot}\LogDataX\{clientCreditAnalysisModel.Id}";
        }

        private string GetLogPathFile(ClientCreditAnalysisModel clientCreditAnalysisModel, string fileName)
        {
            return @$"{this.GetLogPath(clientCreditAnalysisModel)}\{fileName}";
        }
    }
}
