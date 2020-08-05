using IBM.Cloud.SDK.Core.Http;
using ML.Services.IBM.Model;
using ML.Services.IBM.Model.Enums;
using System.Collections.Generic;
using System.IO;

namespace ML.Services.IBM
{
    public partial interface IIBMVisualRecognitionService
    {
        DetailedResponse<ClassifiedImages> Classify(byte[] imagesFile = null, string imagesFilename = null, string imagesFileContentType = null, string url = null, float? threshold = null, List<string> owners = null, List<string> classifierIds = null, string acceptLanguage = null);
        DetailedResponse<ClassifiedImages> Classify(string filePath, IBMImageClassifierEnum ibmImageClassifierEnum);
        DetailedResponse<ClassifiedImages> Classify(byte[] fileMemoryStream, IBMImageClassifierEnum ibmImageClassifierEnum);
        DetailedResponse<ClassifiedImages> Classify(string filePath, List<IBMImageClassifierEnum> ibmImageClassifierEnumList);
    }
}
