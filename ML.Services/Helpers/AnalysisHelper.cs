using ML.Services.Enums;
using System;

namespace ML.Services.Helpers
{
    public static class AnalysisHelper
    {
        public static ScoreRangeEnum GetScore(float value)
        {
            if (value <= 50)
            {
                return ScoreRangeEnum.Green;
            }

            if (value <= 60)
            {
                return ScoreRangeEnum.Grey;
            }

            return ScoreRangeEnum.Red;
        }

        public static ScoreRiskEnum GetScoreRisk(float value)
        {
            if (value < 0.20)
            {
                return ScoreRiskEnum.VeryLow;
            }

            if (value < 0.40)
            {
                return ScoreRiskEnum.Low;
            }

            if (value < 0.60)
            {
                return ScoreRiskEnum.Medium;
            }

            if (value < 0.80)
            {
                return ScoreRiskEnum.High;
            }

            return ScoreRiskEnum.VeryHigh;
        }

        public static ScoreRiskEnum GetScoreRisk(double value)
        {
            var valueConvert = (float)value;

            return AnalysisHelper.GetScoreRisk(valueConvert);
        }
    }
}
