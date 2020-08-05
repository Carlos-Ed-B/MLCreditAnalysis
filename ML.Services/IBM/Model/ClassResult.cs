using Newtonsoft.Json;

namespace ML.Services.IBM.Model
{
    /// <summary>
    /// Result of a class within a classifier.
    /// </summary>
    public class ClassResult
    {
        /// <summary>
        /// Name of the class.
        ///
        /// Class names are translated in the language defined by the **Accept-Language** request header for the
        /// build-in classifier IDs (`default`, `food`, and `explicit`). Class names of custom classifiers are not
        /// translated. The response might not be in the specified language when the requested language is not supported
        /// or when there is no translation for the class name.
        /// </summary>
        [JsonProperty("class", NullValueHandling = NullValueHandling.Ignore)]
        public string _Class { get; set; }
        /// <summary>
        /// Confidence score for the property in the range of 0 to 1. A higher score indicates greater likelihood that
        /// the class is depicted in the image. The default threshold for returning scores from a classifier is 0.5.
        /// </summary>
        [JsonProperty("score", NullValueHandling = NullValueHandling.Ignore)]
        public float? Score { get; set; }
        /// <summary>
        /// Knowledge graph of the property. For example, `/fruit/pome/apple/eating apple/Granny Smith`. Included only
        /// if identified.
        /// </summary>
        [JsonProperty("type_hierarchy", NullValueHandling = NullValueHandling.Ignore)]
        public string TypeHierarchy { get; set; }
    }

}
