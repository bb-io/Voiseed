using Newtonsoft.Json;

namespace Apps.Voiseed.Models
{
    public class LanguageDto
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("code")] public string Code { get; set; }
        [JsonProperty("isTextSupported")] public bool IsTextSupported { get; set; }
        [JsonProperty("isAudioSupported")] public bool IsAudioSupported { get; set; }
    }
}
