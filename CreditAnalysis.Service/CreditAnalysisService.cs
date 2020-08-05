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
        public CreditAnalysisService(IIBMVisualRecognitionService ibmVisualRecognitionService, IAzureVisualRecognitionService azureVisualRecognitionService)
        {
            this._ibmVisualRecognitionService = ibmVisualRecognitionService;
            this._azureVisualRecognitionService = azureVisualRecognitionService;
        }

        public async Task<bool> DoCreditAnalysis(ClientCreditAnalysisModel clientCreditAnalysisModel)
        {
            if (clientCreditAnalysisModel.FileUploadByte == null)
            {
                clientCreditAnalysisModel.FileUploadByte = clientCreditAnalysisModel.FileUpload.ToFileBytes();
            }

            clientCreditAnalysisModel.Id = DateTime.Now.ToFileName();

            this.DoLogOnBegin(clientCreditAnalysisModel);

            var classifyPersonalResultModel = new ClassifyPersonalResultModel
            {
                ClassifyPerson = this.ClassifyPerson(clientCreditAnalysisModel)
            };

            if (!this.IsValid())
            {
                clientCreditAnalysisModel.MessageError = this.GetFirstError();
                return this.IsValid();
            }

            classifyPersonalResultModel.ClassifyExplicitSex = this.ClassifyExplicitSex(clientCreditAnalysisModel);

            if (!this.IsValid())
            {
                clientCreditAnalysisModel.MessageError = this.GetFirstError();
                return this.IsValid();
            }

            classifyPersonalResultModel.VisionFaceResul = await this.ClassifyPersonalData(clientCreditAnalysisModel);
            classifyPersonalResultModel.Status = this.IsValid();
            classifyPersonalResultModel.MessageError = this.GetFirstError();
            clientCreditAnalysisModel.VisionFaceAge = classifyPersonalResultModel.VisionFaceResul.FirstOrDefault().FaceAttributes.Age;
            clientCreditAnalysisModel.VisionFaceGender = classifyPersonalResultModel.VisionFaceResul.FirstOrDefault().FaceAttributes.Gender;

            this.DoLogOnEnd(clientCreditAnalysisModel, classifyPersonalResultModel);

            return this.IsValid();
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

        public async Task<List<VisionFaceResultModel>> ClassifyPersonalData(ClientCreditAnalysisModel clientCreditAnalysisModel, bool saveFaceLandmarksPointOnImage = true)
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
