using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using Newtonsoft.Json;

namespace Apps.Voiseed.Models.Batch
{
    public class BatchResponse
    {
        public string? BulkRequestId { get; set; }
        public BatchDto Batch { get; set; } = new();
        public FileReference ScriptFile { get; set; }
    }

    public class BatchDto
    {
        [JsonProperty("id")][Display("Batch ID")] public string Id { get; set; }
        [JsonProperty("projectId")][Display("Project ID")] public string ProjectId { get; set; }
        [JsonProperty("name")][Display("Name")] public string Name { get; set; }
        [JsonProperty("status")] public string Status { get; set; }
        [JsonProperty("message")] public string? Message { get; set; }
        [JsonProperty("completionPercentage")][Display("Completion percentage")] public int CompletionPercentage { get; set; }
    }
}
