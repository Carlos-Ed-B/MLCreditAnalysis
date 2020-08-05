using CreditAnalysis.Model;
using CreditAnalysis.Model.Enums;
using CreditAnalysis.Service.Interfaces;
using Infrastructure.Layer.Extensions;
using Infrastructure.Layer.Helpers;
using Microsoft.Extensions.DependencyInjection;
using ML.Services.Azure.Interfaces;
using ML.Services.Azure.Models;
using MLCreditAnalysis.Test.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CreditAnalysis.Test.Services
{
    public class CreditAnalysisServiceTest : BaseDITest
    {
        private string DataSourceFolderPath = @"D:\Documents\Dev\MachineLearning\source\MLCreditAnalysis\MLCreditAnalysis.Test\DataSource\";

        [Fact]
        public void DoCreditAnalysisDenied()
        {
            var imagePathNua02 = @$"{DataSourceFolderPath}explicitas\MulherNua02.jpeg";
            var clientCreditAnalysisModel = new ClientCreditAnalysisModel()
            {
                ImagePath = imagePathNua02
            };

            var services = this.ServiceProvider.GetService<ICreditAnalysisService>();
            services.DoCreditAnalysis(clientCreditAnalysisModel);

            Assert.False(services.IsValid());

        }

        [Fact]
        public async Task DoCreditAnalysisSuccessWithFileOnDiskAsync()
        {
            var imagePath = @$"{DataSourceFolderPath}coringa-joaquin-phoenix-01.jpg";

            var clientCreditAnalysisModel = new ClientCreditAnalysisModel()
            {
                Age = 35,
                Gender = GenderEnum.Male,
                FileUploadByte = Helper.ToFileBytes(imagePath)
            };

            var services = this.ServiceProvider.GetService<ICreditAnalysisService>();
            await services.DoCreditAnalysis(clientCreditAnalysisModel);

            Assert.True(services.IsValid());
        }

        [Fact]
        public async Task ClassifyPersonalDataSuccess()
        {
            var imageFace = @$"{DataSourceFolderPath}face_unica.jpg";
            var clientCreditAnalysisModel = new ClientCreditAnalysisModel()
            {
                Age = 22,
                Gender = GenderEnum.Female,
                FileUploadByte = Helper.ToFileBytes(imageFace)
            };

            var services = this.ServiceProvider.GetService<ICreditAnalysisService>();
            await services.ClassifyPersonalData(clientCreditAnalysisModel);

            Assert.True(services.IsValid());
            Assert.True(clientCreditAnalysisModel.ImageFile.Length > 0);
        }

        [Fact]
        public async Task ClassifyPersonalDataFailMultiPeople()
        {
            var imageFace = @$"{DataSourceFolderPath}face_varias.jpg";
            var clientCreditAnalysisModel = new ClientCreditAnalysisModel()
            {
                ImagePath = imageFace,
                Age = 22,
                Gender = GenderEnum.Female
            };

            var services = this.ServiceProvider.GetService<ICreditAnalysisService>();
            await services.ClassifyPersonalData(clientCreditAnalysisModel);

            Assert.True(services.IsValid());
        }

        [Fact]
        public async Task SetFaceLandmarksOnImageWithFileOnDisk()
        {
            string jsonFilePath = @"D:\Documents\Dev\MachineLearning\source\MLCreditAnalysis\MLCreditAnalysis.Test\DataSource\AzureFacePoint.json";
            string imageFilePath = @"D:\Documents\Dev\MachineLearning\source\MLCreditAnalysis\MLCreditAnalysis.Test\DataSource\face_unica.jpg";
            string imageFileNewPath = @"D:\Documents\Dev\MachineLearning\source\MLCreditAnalysis\MLCreditAnalysis.Test\DataSource\face_unica___PONTO.jpg";

            var azudeFaceModel = JsonHelper.DeserializeFromFile<List<VisionFaceResultModel>>(jsonFilePath).FirstOrDefault();

            var services = this.ServiceProvider.GetService<IAzureVisualRecognitionService>();
            services.GetFaceLandmarksPointOnImage(azudeFaceModel, imageFilePath, imageFileNewPath);
        }

        [Fact]
        public async Task SetFaceLandmarksOnImageWithFileStream()
        {
            string jsonFilePath = @"D:\Documents\Dev\MachineLearning\source\MLCreditAnalysis\MLCreditAnalysis.Test\DataSource\AzureFacePoint.json";
            string imageFilePath = @"D:\Documents\Dev\MachineLearning\source\MLCreditAnalysis\MLCreditAnalysis.Test\DataSource\face_unica.jpg";
            string imageFileNewPath = @"D:\Documents\Dev\MachineLearning\source\MLCreditAnalysis\MLCreditAnalysis.Test\DataSource\face_unica___PONTO.jpg";

            var fileStream = Helper.ToMemoryStream(imageFilePath);

            var azudeFaceModel = JsonHelper.DeserializeFromFile<List<VisionFaceResultModel>>(jsonFilePath).FirstOrDefault();

            var services = this.ServiceProvider.GetService<IAzureVisualRecognitionService>();
            services.GetFaceLandmarksPointOnImage(azudeFaceModel, fileStream, imageFileNewPath);
        }

    }
}
