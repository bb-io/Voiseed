using Newtonsoft.Json;

namespace Apps.Voiseed.Models.Project
{
    public class ProjectDto
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("status")] public string Status { get; set; }
        [JsonProperty("type")] public string Type { get; set; }
        [JsonProperty("model")] public string Model { get; set; }
        [JsonProperty("createdAt")] public DateTime CreatedAt { get; set; }
        [JsonProperty("updatedAt")] public DateTime UpdatedAt { get; set; }
    }
}
