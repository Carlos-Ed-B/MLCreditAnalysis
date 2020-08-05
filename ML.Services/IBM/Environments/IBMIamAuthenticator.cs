using IBM.Cloud.SDK.Core.Authentication.Iam;
using ML.Services.IBM.Environments.Interfaces;

namespace ML.Services.IBM.Environments
{
    public class IBMIamAuthenticator: IIBMIamAuthenticator
    {
        public IamAuthenticator Authenticator { get; set; }
    }
}
