using Newtonsoft.Json;

namespace Apps.Voiseed.Models.Speech
{
    public class TextToSpeechUrls
    {
        [JsonProperty("page")] public int Page { get; set; }
        [JsonProperty("limit")] public int Limit { get; set; }
        [JsonProperty("urls")] public List<string> Urls { get; set; }
    }
}
