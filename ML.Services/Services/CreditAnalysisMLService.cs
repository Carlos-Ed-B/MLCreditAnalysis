using Infrastructure.Layer.Base;
using Infrastructure.Layer.Extensions;
using Infrastructure.Layer.Helpers;
using ML.Services.Environments;
using ML.Services.Models;
using ML.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public async Task<double> Classify(CreditAnalysisMLModel creditAnalysisMLModel)
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
                return result.Proba.FirstOrDefault().FirstOrDefault();
            }

            if (creditAnalysisMLModel.ModelType == Enums.CreditAnalysisModelTypeEnum.Modelo02)
            {
                var result = JsonHelper.Deserialize<List<double>>(contentString);
                return result.FirstOrDefault();
            }

            this.AddError("CreditAnalysisModelTypeEnum não informado.");

            return 0;

        }
    }
}
