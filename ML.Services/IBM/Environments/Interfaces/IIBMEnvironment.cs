using System;
using System.Collections.Generic;
using System.Text;

namespace ML.Services.IBM.Environments.Interfaces
{
    public interface IIBMEnvironment
    {
        string VisualRecognitionVersionDate { get; set; }
        string VisualRecognitionApiKey { get; set; }
    }
}
