using ML.Services.Enums;
using ML.Services.Helpers;
using System.Drawing;
using Xunit;

namespace CreditAnalysis.Test.Services.Helpers
{
    public class AnalysisHelperTest
    {
        [Fact]
        public void GetScoreRisk()
        {
            var veryLow = 0.10f;
            var low = 0.30f;
            var medium = 0.50f;
            var high = 0.70f;
            var veryHigh = 0.90f;

            var veryLowScore = AnalysisHelper.GetScoreRisk(veryLow);
            var lowScore = AnalysisHelper.GetScoreRisk(low);
            var mediumScore = AnalysisHelper.GetScoreRisk(medium);
            var highScore = AnalysisHelper.GetScoreRisk(high);
            var veryHighScore = AnalysisHelper.GetScoreRisk(veryHigh);

            Assert.Equal(ScoreRiskEnum.VeryLow, veryLowScore);
            Assert.Equal(ScoreRiskEnum.Low, lowScore);
            Assert.Equal(ScoreRiskEnum.Medium, mediumScore);
            Assert.Equal(ScoreRiskEnum.High, highScore);
            Assert.Equal(ScoreRiskEnum.VeryHigh, veryHighScore);

        }

        [Fact]
        public void DrawString()
        {

            string firstText = "Test";
            string imageFilePath = @"D:\Documents\Dev\MachineLearning\source\MLCreditAnalysis\MLCreditAnalysis.Test\DataSource\face_unica.jpg";
            string imageFileNewPath = @"D:\Documents\Dev\MachineLearning\source\MLCreditAnalysis\MLCreditAnalysis.Test\DataSource\face_unica___PONTO.jpg";
            Bitmap newBitmap;

            using (var bitmap = (Bitmap)Image.FromFile(imageFilePath))//load the image file    
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    Rectangle rect = new Rectangle(504, 202, 5, 5);
                    graphics.DrawRectangle(Pens.Red, rect);
                }

                newBitmap = new Bitmap(bitmap);
            }

            newBitmap.Save(imageFileNewPath);
            newBitmap.Dispose();
        }
    }
}
