using Infrastructure.Layer.Base.Interfaces;
using ML.Services.Models;
using System.Threading.Tasks;

namespace ML.Services.Services.Interfaces
{
    public interface ICreditAnalysisMLService : IBaseCommunicationMessage
    {
        Task<double> Classify(CreditAnalysisMLModel creditAnalysisMLModel);
    }
}
