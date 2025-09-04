using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Apps.Voiseed.Models.Batch
{
    public class BatchDetailsDto
    {
        [JsonProperty("id")]
        [Display("Batch ID")]
        public string Id { get; set; }

        [JsonProperty("projectId")]
        [Display("Project ID")]
        public string ProjectId { get; set; } 

        [JsonProperty("name")]
        [Display("Name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("completionPercentage")]
        [Display("Completion percentage")]
        public int CompletionPercentage { get; set; }

        [JsonProperty("translations")]
        [Display("Translations")]
        public List<BatchTranslationDto> Translations { get; set; } = new();

        [JsonProperty("isCastingCompleted")]
        [Display("Casting completed")]
        public bool IsCastingCompleted { get; set; }

        [JsonProperty("createdAt")]
        [Display("Created at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        [Display("Updated at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken>? AdditionalData { get; set; }
    }

    public class BatchTranslationDto
    {
        [JsonProperty("languageId")]
        [Display("Language ID")]
        public string LanguageId { get; set; }

        [JsonProperty("isCompleted")]
        [Display("Completed")]
        public bool IsCompleted { get; set; }

        [JsonProperty("status")]
        [Display("Status")]
        public string Status { get; set; }
    }
}
