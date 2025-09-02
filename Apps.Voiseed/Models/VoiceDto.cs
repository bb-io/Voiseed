using Newtonsoft.Json;

namespace Apps.Voiseed.Models
{
    public class Voice
    {
        [JsonProperty("name")]   public string Name { get; set; }
        [JsonProperty("age")] public string Age { get; set; }
        [JsonProperty("timbre")] public string Timbre { get; set; }
        [JsonProperty("gender")] public string Gender { get; set; }
    }
}
