using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Voiseed.Models.Project
{
    public class ProjectDto
    {
        [JsonProperty("id")] [Display("Project ID")] public string Id { get; set; }
        [JsonProperty("name")][Display("Name")] public string Name { get; set; }
        [JsonProperty("status")]  public string Status { get; set; }
        [JsonProperty("type")] public string Type { get; set; }
        [JsonProperty("model")] public string Model { get; set; }
        [JsonProperty("createdAt")][Display("Created at")] public DateTime CreatedAt { get; set; }
        [JsonProperty("updatedAt")][Display("Updated at")] public DateTime UpdatedAt { get; set; }
    }
}
