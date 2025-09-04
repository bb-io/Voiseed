using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Voiseed.Models.Speech
{
    public class TextToSpeechStatusResponse
    {
        [JsonProperty("requestId")][Display("Request ID")] public string RequestId { get; set; }
        [JsonProperty("status")] public string Status { get; set; }
        [JsonProperty("progress")] public string? Progress { get; set; }
    }
}
