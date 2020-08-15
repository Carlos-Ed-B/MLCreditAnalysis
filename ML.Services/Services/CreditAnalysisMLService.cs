using Infrastructure.Layer.Base;
using Infrastructure.Layer.Extensions;
using Infrastructure.Layer.Helpers;
using ML.Services.Environments;
using ML.Services.Models;
using ML.Services.Services.Interfaces;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ML.Services.Services
{
    public class CreditAnalysisMLService : BaseService, ICreditAnalysisMLService
    {
        private readonly ICreditAnalysisEnvironment _creditAnalysisEnvironment;
        public CreditAnalysisMLService(ICreditAnalysisEnvironment creditAnalysisEnvironment)
        {
            this._creditAnalysisEnvironment = creditAnalysisEnvironment;
        }

        public async Task<double> ClassifyAsync(CreditAnalysisMLModel creditAnalysisMLModel)
        {
            try
            {
                return await this.DoClassifyAsync(creditAnalysisMLModel);
            }
            catch (System.Exception ex)
            {
                this.AddError($"Erro: {ex.Message}");
                return 0;
            }
        }

        private async Task<double> DoClassifyAsync(CreditAnalysisMLModel creditAnalysisMLModel)
        {
            creditAnalysisMLModel.Escolaridadexxx = new CreditAnalysisDummyModel() { Dummy30876 = creditAnalysisMLModel.Escolaridade };

            var client = new HttpClient();

            var requestParameters = $"predict?model={creditAnalysisMLModel.ModelType.ToDescription()}";

            var uri = $"{this._creditAnalysisEnvironment.CreditAnalysisApi}/{requestParameters}";

            byte[] bytes;
            var json = JsonHelper.Serialize(creditAnalysisMLModel);

            bytes = Encoding.UTF8.GetBytes(json);
            using ByteArrayContent content = new ByteArrayContent(bytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync(uri, content);

            var contentString = await response.Content.ReadAsStringAsync();

            if (creditAnalysisMLModel.ModelType == Enums.CreditAnalysisModelTypeEnum.Modelo01)
            {
                var result = JsonHelper.Deserialize<CreditAnalysisMLResultModel>(contentString);
                return result.Proba.FirstOrDefault().Last();
            }

            if (creditAnalysisMLModel.ModelType == Enums.CreditAnalysisModelTypeEnum.Modelo02)
            {
                var result = JsonHelper.Deserialize<CreditAnalysisML02ResultModel>(contentString);
                return result.prediction.FirstOrDefault();
            }

            this.AddError("CreditAnalysisModelTypeEnum não informado.");

            return 0;

        }
    }
}
