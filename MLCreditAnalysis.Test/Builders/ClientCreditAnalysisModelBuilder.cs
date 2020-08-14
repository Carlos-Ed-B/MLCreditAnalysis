using CreditAnalysis.Model;
using System.Collections.Generic;

namespace CreditAnalysis.Test.Builders
{
    public class ClientCreditAnalysisModelBuilder
    {
        public static IEnumerable<object[]> GetListValid()
        {
            yield return new object[]
            {
                    GetValid()
            };
        }

        public static ClientCreditAnalysisModel GetValid()
        {
            return new ClientCreditAnalysisModel()
            {
                Name = "Barbara, Trujillo",
                Salary = 5000,
                Age = 40,
                Ethnicity = 0,
                Gender = Model.Enums.GenderEnum.Female,
                OwnHome = true,
                ExtraSalary = true,
                MaritalStatus = 0,
                Schooling = 2
            };
        }
    }
}
