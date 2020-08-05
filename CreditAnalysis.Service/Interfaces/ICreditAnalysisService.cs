using CreditAnalysis.Model;
using Infrastructure.Layer.Base.Interfaces;
using ML.Services.Azure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CreditAnalysis.Service.Interfaces
{
    public interface ICreditAnalysisService : IBaseCommunicationMessage
    {
        Task<bool> DoCreditAnalysis(ClientCreditAnalysisModel clientCreditAnalysisModel);
        Task<List<VisionFaceResultModel>> ClassifyPersonalData(ClientCreditAnalysisModel clientCreditAnalysisModel, bool saveFaceLandmarksPointOnImage = true);
    }
}
