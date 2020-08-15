using CreditAnalysis.Model;
using Infrastructure.Layer.Base.Interfaces;
using ML.Services.Azure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CreditAnalysis.Service.Interfaces
{
    public interface ICreditAnalysisService : IBaseCommunicationMessage
    {
        Task<bool> DoCreditAnalysisAsync(ClientCreditAnalysisModel clientCreditAnalysisModel);
        Task<List<VisionFaceResultModel>> ClassifyPersonalDataAsync(ClientCreditAnalysisModel clientCreditAnalysisModel, bool saveFaceLandmarksPointOnImage = true);
    }
}
