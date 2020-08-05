using IBM.Cloud.SDK.Core.Authentication.Iam;

namespace ML.Services.IBM.Environments.Interfaces
{
    public interface IIBMIamAuthenticator
    {
        IamAuthenticator Authenticator { get; set; }
    }
}
