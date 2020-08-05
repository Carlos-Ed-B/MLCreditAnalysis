using Infrastructure.Layer.Helpers;
using Microsoft.Extensions.DependencyInjection;
using ML.Services.Azure.Interfaces;
using ML.Services.Azure.Models;
using MLCreditAnalysis.Test.Base;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace CreditAnalysis.Test.Azure
{
    public class AzureCognitiveServicesVisionFaceServiceTest : BaseDITest
    {
        // replace with the path file
        private string DataSourceFolderPath = @"D:\Documents\Dev\MachineLearning\source\MLCreditAnalysis\MLCreditAnalysis.Test\DataSource\";
        // replace with the string found in your key
        const string subscriptionKey = "";
        // replace  with the string found in your endpoint URL
        const string uriBase = "https://###.cognitiveservices.azure.com/face/v1.0/detect";


        [Fact]
        public async Task AnalyzeFromDiskAsync()
        {
            var imageFace = @$"{DataSourceFolderPath}face_unica.jpg";

            var service = this.ServiceProvider.GetService<IAzureVisualRecognitionService>();
            var result = await service.Classify(imageFace);

            Assert.True(result.Any());

        }


        [Fact]
        public async Task MakeAnalysisRequestTestAsync()
        {
            var imageFace = @$"{DataSourceFolderPath}face_unica.jpg";
            await this.MakeAnalysisRequest(imageFace);
        }

        // Gets the analysis of the specified image by using the Face REST API.
        private async Task MakeAnalysisRequest(string imageFilePath)
        {
            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false" +
                "&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses," +
                "emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";

            // Assemble the URI for the REST API Call.
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response;

            // Request body. Posts a locally stored JPEG image.
            byte[] byteData = this.GetImageAsByteArray(imageFilePath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);

                string contentString = await response.Content.ReadAsStringAsync();
                var model = JsonHelper.Deserialize<List<VisionFaceResultModel>>(contentString);

            }
        }

        // Returns the contents of the specified file as a byte array.
        private byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
    }
}

