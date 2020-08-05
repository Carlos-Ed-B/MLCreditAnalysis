using IBM.Cloud.SDK.Core.Http;
using IBM.Cloud.SDK.Core.Service;
using IBM.Watson;
using Infrastructure.Layer.Extensions;
using Infrastructure.Layer.Helpers;
using ML.Services.IBM.Environments.Interfaces;
using ML.Services.IBM.Model;
using ML.Services.IBM.Model.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace ML.Services.IBM
{
    public partial class IBMVisualRecognitionService : IBMService, IIBMVisualRecognitionService
    {
        private const string _serviceName = "visual_recognition";
        private const string _defaultServiceUrl = "https://gateway.watsonplatform.net/visual-recognition/api";
        private readonly IIBMEnvironment _ibmMEnvironment;

        public string VersionDate { get { return this._ibmMEnvironment.VisualRecognitionVersionDate; } }

        public IBMVisualRecognitionService(IIBMIamAuthenticator ibmIamAuthenticator, IIBMEnvironment ibmMEnvironment) : base(_serviceName, ibmIamAuthenticator.Authenticator)
        {
            this._ibmMEnvironment = ibmMEnvironment;

            if (string.IsNullOrEmpty(ServiceUrl))
            {
                SetServiceUrl(_defaultServiceUrl);
            }
        }

        public DetailedResponse<ClassifiedImages> Classify(byte[] imagesFile = null, string imagesFilename = null, string imagesFileContentType = null, string url = null, float? threshold = null, List<string> owners = null, List<string> classifierIds = null, string acceptLanguage = null)
        {
            try
            {
                var formData = this.FillMultipartFormDataContent(imagesFile, imagesFilename, imagesFileContentType, url, threshold, owners, classifierIds);

                base.SetAuthentication();

                IClient client = this.Client;
                var restRequest = client.PostAsync($"{this.Endpoint}/v3/classify");

                restRequest.WithArgument("version", VersionDate);
                restRequest.WithHeader("Accept", "application/json");

                if (!string.IsNullOrEmpty(acceptLanguage))
                {
                    restRequest.WithHeader("Accept-Language", acceptLanguage);
                }

                restRequest.WithBodyContent(formData);

                restRequest.WithHeaders(Common.GetSdkHeaders("watson_vision_combined", "v3", "Classify"));
                restRequest.WithHeaders(customRequestHeaders);
                base.ClearCustomRequestHeaders();

                var result = restRequest.As<ClassifiedImages>().Result;

                if (result == null)
                {
                    result = new DetailedResponse<ClassifiedImages>();
                }

                return result;
            }
            catch (AggregateException ae)
            {
                throw ae.Flatten();
            }
        }

        public DetailedResponse<ClassifiedImages> Classify(string filePath, IBMImageClassifierEnum ibmImageClassifierEnum)
        {
            var fileName = "teste.jpg";
            var fileBytes = Helper.ToFileBytes(filePath);

            return this.Classify(fileBytes, imagesFilename: fileName, classifierIds: new List<string>() { ibmImageClassifierEnum.ToDescription() });
        }

        public DetailedResponse<ClassifiedImages> Classify(byte[] fileMemoryStream, IBMImageClassifierEnum ibmImageClassifierEnum)
        {
            var fileName = "teste.jpg";

            return this.Classify(fileMemoryStream, imagesFilename: fileName, classifierIds: new List<string>() { ibmImageClassifierEnum.ToDescription() });
        }

        public DetailedResponse<ClassifiedImages> Classify(string filePath, List<IBMImageClassifierEnum> ibmImageClassifierEnumList)
        {
            var fileName = "teste.jpg";
            var fileBytes = Helper.ToFileBytes(filePath);
            var classifierList = new List<string>();

            foreach (var ibmImageClassifierEnum in ibmImageClassifierEnumList)
            {
                classifierList.Add(ibmImageClassifierEnum.ToDescription());
            }

            return this.Classify(fileBytes, imagesFilename: fileName, classifierIds: classifierList);
        }

        private MultipartFormDataContent FillMultipartFormDataContent(byte[] imagesFile, string imagesFilename, string imagesFileContentType, string url, float? threshold, List<string> owners, List<string> classifierIds)
        {
            var formData = new MultipartFormDataContent();

            if (imagesFile != null)
            {
                var imagesFileContent = new ByteArrayContent(imagesFile);
                System.Net.Http.Headers.MediaTypeHeaderValue contentType;
                System.Net.Http.Headers.MediaTypeHeaderValue.TryParse(imagesFileContentType, out contentType);
                imagesFileContent.Headers.ContentType = contentType;
                formData.Add(imagesFileContent, "images_file", imagesFilename);
            }

            if (url != null)
            {
                var urlContent = new StringContent(url, Encoding.UTF8, HttpMediaType.TEXT_PLAIN);
                urlContent.Headers.ContentType = null;
                formData.Add(urlContent, "url");
            }

            if (threshold != null)
            {
                var thresholdContent = new StringContent(threshold.ToString(), Encoding.UTF8, HttpMediaType.TEXT_PLAIN);
                thresholdContent.Headers.ContentType = null;
                formData.Add(thresholdContent, "threshold");
            }

            if (owners != null)
            {
                var ownersContent = new StringContent(string.Join(", ", owners.ToArray()), Encoding.UTF8, HttpMediaType.TEXT_PLAIN);
                ownersContent.Headers.ContentType = null;
                formData.Add(ownersContent, "owners");
            }

            if (classifierIds != null)
            {
                var classifierIdsContent = new StringContent(string.Join(", ", classifierIds.ToArray()), Encoding.UTF8, HttpMediaType.TEXT_PLAIN);
                classifierIdsContent.Headers.ContentType = null;
                formData.Add(classifierIdsContent, "classifier_ids");
            }

            return formData;
        }
    }
}
