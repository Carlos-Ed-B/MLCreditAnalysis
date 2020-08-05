using ML.Services.IBM.Environments.Interfaces;

namespace ML.Services.IBM.Environments
{
    public class IBMEnvironment : IIBMEnvironment
    {
        public string VisualRecognitionVersionDate { get; set; }
        public string VisualRecognitionApiKey { get; set; }
    }
}
