using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Voiseed.Models.Speech
{
    public class TextToSpeechResponse
    {
        [JsonProperty("requestId")]
        [Display("Request ID")]
        public string RequestId { get; set; }
    }
}
