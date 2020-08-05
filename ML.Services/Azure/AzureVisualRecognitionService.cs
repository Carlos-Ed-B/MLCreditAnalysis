using Infrastructure.Layer.Extensions;
using Infrastructure.Layer.Helpers;
using ML.Services.Azure.Environments.Interfaces;
using ML.Services.Azure.Interfaces;
using ML.Services.Azure.Models;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ML.Services.Azure
{
    public class AzureVisualRecognitionService : IAzureVisualRecognitionService
    {
        private readonly IAzureEnvironment _azureEnvironment;

        public AzureVisualRecognitionService(IAzureEnvironment azureEnvironment)
        {
            this._azureEnvironment = azureEnvironment;
        }

        private string GetVisualRecognitionApiUrl()
        {
            return $"https://{this._azureEnvironment.VisualRecognitionApiUrl}.cognitiveservices.azure.com/face/v1.0/detect";
        }

        public async Task<List<VisionFaceResultModel>> Classify(string imageFilePath)
        {
            var byteData = Helper.GetImageAsByteArray(imageFilePath);

            return await this.Classify(byteData);
        }

        public async Task<List<VisionFaceResultModel>> Classify(MemoryStream memoryStream)
        {
            var byteData = memoryStream.ToArray();

            return await this.Classify(byteData);

        }

        public async Task<List<VisionFaceResultModel>> Classify(byte[] byteData)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", this._azureEnvironment.VisualRecognitionApiKey);

            var requestParameters = "returnFaceId=true&returnFaceLandmarks=true" +
                "&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses," +
                "emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";

            var uri = $"{this.GetVisualRecognitionApiUrl()}?{requestParameters}";

            using ByteArrayContent content = new ByteArrayContent(byteData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            var response = await client.PostAsync(uri, content);

            var contentString = await response.Content.ReadAsStringAsync();

            return JsonHelper.Deserialize<List<VisionFaceResultModel>>(contentString);
        }

        public byte[] GetFaceLandmarksPointOnImage(VisionFaceResultModel azudeFaceModel, byte[] imageFileByte, string imageFileNewPath)
        {
            Bitmap newBitmap;

            using (var ms = new MemoryStream(imageFileByte))
            {
                using (var bitmap = (Bitmap)Image.FromStream(ms))
                {
                    newBitmap = this.ToBitmapFaceLandmarksPointOnImage(azudeFaceModel, bitmap);
                }
            }

            var imageResize = ImageHelper.GetResizeImage(newBitmap, 600, 600);

            var imageFile = Helper.ImageToByte(imageResize);

            if (imageFileNewPath.HasValue())
            {
                imageResize.Save(imageFileNewPath);
            }

            newBitmap.Dispose();

            return imageFile;
        }

        public byte[] GetFaceLandmarksPointOnImage(VisionFaceResultModel azudeFaceModel, MemoryStream imageFileMemoryStream, string imageFileNewPath)
        {
            return this.GetFaceLandmarksPointOnImage(azudeFaceModel, imageFileMemoryStream.ToArray(), imageFileNewPath);
        }

        public byte[] GetFaceLandmarksPointOnImage(VisionFaceResultModel azudeFaceModel, string imageFilePath, string imageFileNewPath)
        {
            var imageFileByte = Helper.ToFileBytes(imageFilePath);
            return this.GetFaceLandmarksPointOnImage(azudeFaceModel, imageFileByte, imageFileNewPath);
        }

        private Bitmap ToBitmapFaceLandmarksPointOnImage(VisionFaceResultModel azudeFaceModel, Bitmap bitmap)
        {
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                var faceBox = new Rectangle(
                        azudeFaceModel.FaceRectangle.Left.ToInteger(),
                        azudeFaceModel.FaceRectangle.Top.ToInteger(),
                        azudeFaceModel.FaceRectangle.Width.ToInteger(),
                        azudeFaceModel.FaceRectangle.Height.ToInteger());

                graphics.DrawRectangle(Pens.Yellow, faceBox);


                foreach (var faceLandmarkTemp in azudeFaceModel.FaceLandmarks)
                {
                    var rect = new Rectangle(faceLandmarkTemp.Value.X.ToInteger(), faceLandmarkTemp.Value.Y.ToInteger(), 3, 3);
                    graphics.DrawRectangle(Pens.Red, rect);
                }
            }

            return new Bitmap(bitmap);
        }
    }
}
