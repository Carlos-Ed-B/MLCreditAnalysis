using ML.Services.Azure.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ML.Services.Azure.Interfaces
{
    public interface IAzureVisualRecognitionService
    {
        Task<List<VisionFaceResultModel>> Classify(string imageFilePath);
        Task<List<VisionFaceResultModel>> Classify(byte[] byteData);
        Task<List<VisionFaceResultModel>> Classify(MemoryStream memoryStream);
        byte[] GetFaceLandmarksPointOnImage(VisionFaceResultModel azudeFaceModel, string imageFilePath, string imageFileNewPath);
        byte[] GetFaceLandmarksPointOnImage(VisionFaceResultModel azudeFaceModel, byte[] imageFileByte, string imageFileNewPath);
        byte[] GetFaceLandmarksPointOnImage(VisionFaceResultModel azudeFaceModel, MemoryStream imageFileMemoryStream, string imageFileNewPath);
    }
}
