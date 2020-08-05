using Newtonsoft.Json;

namespace ML.Services.IBM.Model
{
    /// <summary>
    /// Information about what might have caused a failure, such as an image that is too large. Not returned when there
    /// is no error.
    /// </summary>
    public class ErrorInfo
    {
        /// <summary>
        /// HTTP status code.
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public long? Code { get; set; }
        /// <summary>
        /// Human-readable error description. For example, `File size limit exceeded`.
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        /// <summary>
        /// Codified error string. For example, `limit_exceeded`.
        /// </summary>
        [JsonProperty("error_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorId { get; set; }
    }

}
