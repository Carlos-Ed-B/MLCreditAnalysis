using Newtonsoft.Json;

namespace ML.Services.IBM.Model
{
    /// <summary>
    /// Information about something that went wrong.
    /// </summary>
    public class WarningInfo
    {
        /// <summary>
        /// Codified warning string, such as `limit_reached`.
        /// </summary>
        [JsonProperty("warning_id", NullValueHandling = NullValueHandling.Ignore)]
        public string WarningId { get; set; }
        /// <summary>
        /// Information about the error.
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    }

}
