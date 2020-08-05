using System.Collections.Generic;
using Newtonsoft.Json;

namespace ML.Services.IBM.Model
{
    /// <summary>
    /// Classifier and score combination.
    /// </summary>
    public class ClassifierResult
    {
        /// <summary>
        /// Name of the classifier.
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        /// <summary>
        /// ID of a classifier identified in the image.
        /// </summary>
        [JsonProperty("classifier_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ClassifierId { get; set; }
        /// <summary>
        /// Classes within the classifier.
        /// </summary>
        [JsonProperty("classes", NullValueHandling = NullValueHandling.Ignore)]
        public List<ClassResult> Classes { get; set; }
    }

}
