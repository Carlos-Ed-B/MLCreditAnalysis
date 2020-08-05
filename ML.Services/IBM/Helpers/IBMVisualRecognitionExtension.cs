using IBM.Cloud.SDK.Core.Http;
using Infrastructure.Layer.Extensions;
using ML.Services.IBM.Model;
using ML.Services.IBM.Model.Enums;
using System.Linq;

namespace ML.Services.IBM.Helpers
{
    public static class IBMVisualRecognitionExtension
    {
        public static ClassResult GetClassifierClassResult(this DetailedResponse<ClassifiedImages> classifyResult, IBMImageClassifierEnum classifierEnum)
        {
            foreach (var image in classifyResult.Result.Images)
            {
                foreach (var classifier in image.Classifiers)
                {
                    return classifier.Classes.FirstOrDefault(x => x._Class.ToLower().Equals(classifierEnum.ToDescription().ToLower()));
                }
            }

            return null;
        }

        public static bool IsClassifiersType(ClassifierResult classifyResult, IBMImageClassifierEnum classifierEnum)
        {
            return classifyResult.Classes.Any(x => x._Class.ToLower().Equals(classifierEnum.ToDescription().ToLower()));
        }

        public static bool IsPerson(DetailedResponse<ClassifiedImages> classifyResult)
        {
            return classifyResult.Result.Images[0].Classifiers[0].Classes.Any(x => x._Class.ToLower().Equals(IBMImageClassifierEnum.Person.ToDescription().ToLower()));
        }

        public static bool IsClassifiersType(DetailedResponse<ClassifiedImages> classifyResult, IBMImageClassifierEnum classifierEnum)
        {
            return classifyResult.Result.Images[0].Classifiers[0].Classes.Any(x => x._Class.ToLower().Equals(IBMImageClassifierEnum.Person.ToDescription().ToLower()));
        }

        public static bool HasClassifiers(DetailedResponse<ClassifiedImages> classifyResult)
        {
            if (!classifyResult.Result.Images.Any())
            {
                return false;
            }

            if (!classifyResult.Result.Images.FirstOrDefault().Classifiers.Any())
            {
                return false;
            }

            return true;
        }
    }
}
