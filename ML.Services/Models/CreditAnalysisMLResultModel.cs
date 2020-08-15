using Newtonsoft.Json;
using System.Collections.Generic;

namespace ML.Services.Models
{
    public class CreditAnalysisMLResultModel
    {
        [JsonProperty("prediction")]
        public List<int> Prediction { get; set; }

        [JsonProperty("proba")]
        public List<List<double>> Proba { get; set; }
    }

    public class CreditAnalysisML02ResultModel
    {
        public List<double> prediction { get; set; }
    }

}
