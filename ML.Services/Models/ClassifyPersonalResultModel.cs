using IBM.Cloud.SDK.Core.Http;
using ML.Services.Azure.Models;
using ML.Services.IBM.Model;
using System.Collections.Generic;

namespace ML.Services.Models
{
    public class ClassifyPersonalResultModel
    {
        public DetailedResponse<ClassifiedImages> ClassifyPerson { get; set; }
        public DetailedResponse<ClassifiedImages> ClassifyExplicitSex { get; set; }
        public List<VisionFaceResultModel> VisionFaceResul { get; set; }
        public bool Status { get; set; }
        public string MessageError { get; set; }
    }
}
