using ML.Services.Models;
using System.Collections.Generic;

namespace CreditAnalysis.Test.Builders
{
    public static class CreditAnalysisMLModelBuilder
    {
        public static IEnumerable<object[]> GetListValid()
        {
            yield return new object[]
            {
                    GetValid()
            };
        }

        public static CreditAnalysisMLModel GetValid()
        {
            return new CreditAnalysisMLModel()
            {
                Nome = "Barbara, Trujillo",
                Renda = 0,
                Idade = 40,
                Etnia = 0,
                Sexo = 1,
                Casapropria = 0,
                Outrasrendas = 0,
                Estadocivil = 0,
                Escolaridade = 2,
                Escolaridadexxx = new CreditAnalysisDummyModel() { Dummy30876 = 2 },
            };
        }
    }
}
