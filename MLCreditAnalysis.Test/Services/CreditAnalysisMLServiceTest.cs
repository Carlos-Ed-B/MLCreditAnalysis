using CreditAnalysis.Test.Builders;
using Microsoft.Extensions.DependencyInjection;
using ML.Services.Enums;
using ML.Services.Models;
using ML.Services.Services.Interfaces;
using MLCreditAnalysis.Test.Base;
using System.Threading.Tasks;
using Xunit;

namespace CreditAnalysis.Test.Services
{
    public class CreditAnalysisMLServiceTest : BaseDITest
    {
        [Theory]
        [MemberData(nameof(CreditAnalysisMLModelBuilder.GetListValid), MemberType = typeof(CreditAnalysisMLModelBuilder))]
        public async Task ClassifyWithModelo01(CreditAnalysisMLModel creditAnalysisMLModel)
        {
            creditAnalysisMLModel.ModelType = CreditAnalysisModelTypeEnum.Modelo01;

            var services = this.ServiceProvider.GetService<ICreditAnalysisMLService>();
            var result = await services.ClassifyAsync(creditAnalysisMLModel);

            Assert.True(result > 0);
            Assert.True(services.IsValid());
        }

        [Theory]
        [MemberData(nameof(CreditAnalysisMLModelBuilder.GetListValid), MemberType = typeof(CreditAnalysisMLModelBuilder))]
        public async Task ClassifyWithModelo02(CreditAnalysisMLModel creditAnalysisMLModel)
        {
            creditAnalysisMLModel.ModelType = CreditAnalysisModelTypeEnum.Modelo02;

            var services = this.ServiceProvider.GetService<ICreditAnalysisMLService>();
            var result = await services.ClassifyAsync(creditAnalysisMLModel);

            Assert.True(result > 0);
            Assert.True(services.IsValid());
        }
    }
}
