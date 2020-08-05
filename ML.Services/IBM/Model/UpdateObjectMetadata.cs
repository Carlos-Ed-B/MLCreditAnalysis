using Newtonsoft.Json;

namespace ML.Services.IBM.Model
{
    /// <summary>
    /// Basic information about an updated object.
    /// </summary>
    public class UpdateObjectMetadata
    {
        /// <summary>
        /// The updated name of the object. The name can contain alphanumeric, underscore, hyphen, space, and dot
        /// characters. It cannot begin with the reserved prefix `sys-`.
        /// </summary>
        [JsonProperty("object", NullValueHandling = NullValueHandling.Ignore)]
        public string _Object { get; set; }
        /// <summary>
        /// Number of bounding boxes in the collection with the updated object name.
        /// </summary>
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public virtual long? Count { get; private set; }
    }

}
