namespace ML.Services.Azure.Environments.Interfaces
{
    public interface IAzureEnvironment
    {
        string VisualRecognitionApiKey { get; set; }
        string VisualRecognitionApiUrl { get; set; }
    }
}
