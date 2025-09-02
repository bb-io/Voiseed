using Newtonsoft.Json;

namespace Apps.Voiseed.Models
{
    public class ModelDto
    {
        [JsonProperty("model")] public string Model { get; set; }
        [JsonProperty("description")] public string Description { get; set; }
        [JsonProperty("defaultOutputBitrate")] public string DefaultOutputBitrate { get; set; }
        [JsonProperty("defaultOutputSamplingRate")] public string DefaultOutputSamplingRate { get; set; }
    }
}
