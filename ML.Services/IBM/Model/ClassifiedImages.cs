using System.Collections.Generic;
using Newtonsoft.Json;

namespace ML.Services.IBM.Model
{
    /// <summary>
    /// Results for all images.
    /// </summary>
    public class ClassifiedImages
    {
        /// <summary>
        /// Number of custom classes identified in the images.
        /// </summary>
        [JsonProperty("custom_classes", NullValueHandling = NullValueHandling.Ignore)]
        public long? CustomClasses { get; set; }
        /// <summary>
        /// Number of images processed for the API call.
        /// </summary>
        [JsonProperty("images_processed", NullValueHandling = NullValueHandling.Ignore)]
        public long? ImagesProcessed { get; set; }
        /// <summary>
        /// Classified images.
        /// </summary>
        [JsonProperty("images", NullValueHandling = NullValueHandling.Ignore)]
        public List<ClassifiedImage> Images { get; set; }
        /// <summary>
        /// Information about what might cause less than optimal output. For example, a request sent with a corrupt .zip
        /// file and a list of image URLs will still complete, but does not return the expected output. Not returned
        /// when there is no warning.
        /// </summary>
        [JsonProperty("warnings", NullValueHandling = NullValueHandling.Ignore)]
        public List<WarningInfo> Warnings { get; set; }
    }

}
