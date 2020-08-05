using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ML.Services.Azure.Models
{

    public partial class VisionFaceResultModel
    {
        [JsonProperty("faceId")]
        public Guid FaceId { get; set; }

        [JsonProperty("faceRectangle")]
        public FaceRectangle FaceRectangle { get; set; }

        [JsonProperty("faceLandmarks")]
        public Dictionary<string, FaceLandmark> FaceLandmarks { get; set; }

        [JsonProperty("faceAttributes")]
        public FaceAttributes FaceAttributes { get; set; }
    }

    public partial class FaceAttributes
    {
        [JsonProperty("smile")]
        public long Smile { get; set; }

        [JsonProperty("headPose")]
        public HeadPose HeadPose { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("age")]
        public long Age { get; set; }

        [JsonProperty("facialHair")]
        public FacialHair FacialHair { get; set; }

        [JsonProperty("glasses")]
        public string Glasses { get; set; }

        [JsonProperty("emotion")]
        public Emotion Emotion { get; set; }

        [JsonProperty("blur")]
        public Blur Blur { get; set; }

        [JsonProperty("exposure")]
        public Exposure Exposure { get; set; }

        [JsonProperty("noise")]
        public Noise Noise { get; set; }

        [JsonProperty("makeup")]
        public Makeup Makeup { get; set; }

        [JsonProperty("accessories")]
        public List<Accessory> Accessories { get; set; }

        [JsonProperty("occlusion")]
        public Occlusion Occlusion { get; set; }

        [JsonProperty("hair")]
        public Hair Hair { get; set; }
    }

    public partial class Accessory
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("confidence")]
        public long Confidence { get; set; }
    }

    public partial class Blur
    {
        [JsonProperty("blurLevel")]
        public string BlurLevel { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }
    }

    public partial class Emotion
    {
        [JsonProperty("anger")]
        public long Anger { get; set; }

        [JsonProperty("contempt")]
        public long Contempt { get; set; }

        [JsonProperty("disgust")]
        public long Disgust { get; set; }

        [JsonProperty("fear")]
        public long Fear { get; set; }

        [JsonProperty("happiness")]
        public long Happiness { get; set; }

        [JsonProperty("neutral")]
        public long Neutral { get; set; }

        [JsonProperty("sadness")]
        public long Sadness { get; set; }

        [JsonProperty("surprise")]
        public long Surprise { get; set; }
    }

    public partial class Exposure
    {
        [JsonProperty("exposureLevel")]
        public string ExposureLevel { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }

    public partial class FacialHair
    {
        [JsonProperty("moustache")]
        public long Moustache { get; set; }

        [JsonProperty("beard")]
        public long Beard { get; set; }

        [JsonProperty("sideburns")]
        public long Sideburns { get; set; }
    }

    public partial class Hair
    {
        [JsonProperty("bald")]
        public double Bald { get; set; }

        [JsonProperty("invisible")]
        public bool Invisible { get; set; }

        [JsonProperty("hairColor")]
        public List<HairColor> HairColor { get; set; }
    }

    public partial class HairColor
    {
        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("confidence")]
        public double Confidence { get; set; }
    }

    public partial class HeadPose
    {
        [JsonProperty("pitch")]
        public long Pitch { get; set; }

        [JsonProperty("roll")]
        public double Roll { get; set; }

        [JsonProperty("yaw")]
        public long Yaw { get; set; }
    }

    public partial class Makeup
    {
        [JsonProperty("eyeMakeup")]
        public bool EyeMakeup { get; set; }

        [JsonProperty("lipMakeup")]
        public bool LipMakeup { get; set; }
    }

    public partial class Noise
    {
        [JsonProperty("noiseLevel")]
        public string NoiseLevel { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }

    public partial class Occlusion
    {
        [JsonProperty("foreheadOccluded")]
        public bool ForeheadOccluded { get; set; }

        [JsonProperty("eyeOccluded")]
        public bool EyeOccluded { get; set; }

        [JsonProperty("mouthOccluded")]
        public bool MouthOccluded { get; set; }
    }

    public partial class FaceLandmark
    {
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }
    }

    public partial class FaceRectangle
    {
        [JsonProperty("top")]
        public long Top { get; set; }

        [JsonProperty("left")]
        public long Left { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }
    }
}
