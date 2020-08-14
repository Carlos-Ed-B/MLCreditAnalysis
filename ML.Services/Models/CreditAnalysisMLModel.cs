using ML.Services.Enums;
using Newtonsoft.Json;

namespace ML.Services.Models
{
    public class CreditAnalysisMLModel
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("renda")]
        public double Renda { get; set; }

        [JsonProperty("idade")]
        public double Idade { get; set; }

        [JsonProperty("etnia")]
        public int Etnia { get; set; }

        [JsonProperty("sexo")]
        public int Sexo { get; set; }

        [JsonProperty("casapropria")]
        public int Casapropria { get; set; }

        [JsonProperty("outrasrendas")]
        public int Outrasrendas { get; set; }

        [JsonProperty("estadocivil")]
        public int Estadocivil { get; set; }

        [JsonProperty("escolaridade")]
        public int Escolaridade { get; set; }

        [JsonProperty("escolaridadexxx")]
        public CreditAnalysisDummyModel Escolaridadexxx { get; set; }

        [JsonIgnore]
        public CreditAnalysisModelTypeEnum ModelType { get; set; }
    }

    public class CreditAnalysisDummyModel
    {
        [JsonProperty("30876")]
        public int Dummy30876 { get; set; }
    }
}
