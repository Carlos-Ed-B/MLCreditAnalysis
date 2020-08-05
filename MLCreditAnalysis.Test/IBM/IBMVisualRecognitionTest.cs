using IBM.Cloud.SDK.Core.Authentication.Iam;
using Microsoft.Extensions.DependencyInjection;
using ML.Services.IBM;
using ML.Services.IBM.Environments;
using ML.Services.IBM.Model.Enums;
using MLCreditAnalysis.Test.Base;
using System.Collections.Generic;
using Xunit;

namespace MLCreditAnalysis.Test.IBM
{
    public class IBMVisualRecognitionTest : BaseDITest
    {
        private string DataSourceFolderPath = @"D:\Documents\Dev\MachineLearning\source\MLCreditAnalysis\MLCreditAnalysis.Test\DataSource\";

        [Fact]
        public void AnalyzeFromDisk()
        {
            var imagePathNua02 = @$"{DataSourceFolderPath}explicitas\MulherNua02.jpeg";

            var business = this.ServiceProvider.GetService<IIBMVisualRecognitionService>();
            var classifyResult = business.Classify(imagePathNua02, IBMImageClassifierEnum.ExplicitSex);
            var classResult = classifyResult.Result.Images[0].Classifiers[0].Classes[0];
        }

        [Fact]
        public void AnalyzeFromDiskWithMultiClassifier()
        {
            var imagePathNua02 = @$"{DataSourceFolderPath}explicitas\MulherNua02.jpeg";
            var ibmImageClassifierEnumList = new List<IBMImageClassifierEnum>
            {
                IBMImageClassifierEnum.ExplicitSex,
                IBMImageClassifierEnum.Default
            };

            var business = this.ServiceProvider.GetService<IIBMVisualRecognitionService>();
            var classifyResult = business.Classify(imagePathNua02, ibmImageClassifierEnumList);
            var classResult = classifyResult.Result.Images[0].Classifiers[0].Classes[0];
        }

        [Fact]
        public void AnalyzeMultiFile()
        {
            var classifier_default_ids = "default";
            var classifier_explicit_ids = "explicit";
            var classifier_food_ids = "food";

            string apiKey = "";
            string versionDate = "2018-03-19";


            var imagePath01 = $"{DataSourceFolderPath}coringa-joaquin-phoenix-01.jpg";
            var imagePath02 = $"{DataSourceFolderPath}coringa-joaquin-phoenix-02.jpg";
            var imagePathbandido = @$"{DataSourceFolderPath}explicitas\bandido.jpg";
            var imagePathHentai = @$"{DataSourceFolderPath}explicitas\Hentai.jpg";
            var imagePathNua01 = @$"{DataSourceFolderPath}explicitas\MulherNua01.jpeg";
            var imagePathNua02 = @$"{DataSourceFolderPath}explicitas\MulherNua02.jpeg";

            IamAuthenticator authenticator = new IamAuthenticator(apikey: apiKey);
            var ibmIamAuthenticator = new IBMIamAuthenticator
            {
                Authenticator = authenticator
            };

            var ibmEnvironment = new IBMEnvironment()
            {
                VisualRecognitionVersionDate = "2018-03-19",
                VisualRecognitionApiKey = apiKey
            };

            IBMVisualRecognitionService visualRecognition = new IBMVisualRecognitionService(ibmIamAuthenticator, ibmEnvironment);

            visualRecognition.DisableSslVerification(true);

            var resultImageWeb = visualRecognition.Classify(url: "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d9/Collage_of_Nine_Dogs.jpg/1023px-Collage_of_Nine_Dogs.jpg");


            //var fileWithMetadata = Helper.ToMemoryStream(imagePath01);
            //var fileWithMetadataBandido = Helper.ToMemoryStream(imagePathbandido);
            //var fileWithMetadataHentai = Helper.ToMemoryStream(imagePathHentai);
            //var fileWithMetadataNua01 = Helper.ToMemoryStream(imagePathNua01);
            //var fileWithMetadataNua02 = Helper.ToMemoryStream(imagePathNua02);

            //var resultImageCoringaLocal = visualRecognition.Classify(fileWithMetadata, "coringa-joaquin-phoenix-01.jpg");
            //var resultImageBandidoLocal = visualRecognition.Classify(fileWithMetadataBandido, "bandido.jpg", classifierIds: new List<string>() { classifier_default_ids });
            //var resultImageHentaiLocal = visualRecognition.Classify(fileWithMetadataHentai, "Hentai.jpg", classifierIds: new List<string>() { classifier_default_ids });
            //var resultImageCoringaXLocal = visualRecognition.Classify(fileWithMetadata, "coringa.jpg", classifierIds: new List<string>() { classifier_default_ids });
            //var resultImageMulherNua01Local = visualRecognition.Classify(fileWithMetadataNua01, "MulherNua01.jpg", classifierIds: new List<string>() { classifier_default_ids });
            //var resultImageMulherNua02Local = visualRecognition.Classify(fileWithMetadataNua02, "MulherNua02.jpg", classifierIds: new List<string>() { classifier_default_ids });

        }

    }
}


