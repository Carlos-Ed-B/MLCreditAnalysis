using ML.Services.Azure.Environments.Interfaces;

namespace ML.Services.Azure.Environments
{
    public class AzureEnvironment : IAzureEnvironment
    {
        public string VisualRecognitionApiKey { get; set; }
        public string VisualRecognitionApiUrl { get; set; }
    }
}
