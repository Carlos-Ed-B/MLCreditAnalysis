using System.Collections.Generic;
using Newtonsoft.Json;

namespace ML.Services.IBM.Model
{
    /// <summary>
    /// Results for one image.
    /// </summary>
    public class ClassifiedImage
    {
        /// <summary>
        /// Source of the image before any redirects. Not returned when the image is uploaded.
        /// </summary>
        [JsonProperty("source_url", NullValueHandling = NullValueHandling.Ignore)]
        public string SourceUrl { get; set; }
        /// <summary>
        /// Fully resolved URL of the image after redirects are followed. Not returned when the image is uploaded.
        /// </summary>
        [JsonProperty("resolved_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ResolvedUrl { get; set; }
        /// <summary>
        /// Relative path of the image file if uploaded directly. Not returned when the image is passed by URL.
        /// </summary>
        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public string Image { get; set; }
        /// <summary>
        /// Information about what might have caused a failure, such as an image that is too large. Not returned when
        /// there is no error.
        /// </summary>
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public ErrorInfo Error { get; set; }
        /// <summary>
        /// The classifiers.
        /// </summary>
        [JsonProperty("classifiers", NullValueHandling = NullValueHandling.Ignore)]
        public List<ClassifierResult> Classifiers { get; set; }
    }

}
