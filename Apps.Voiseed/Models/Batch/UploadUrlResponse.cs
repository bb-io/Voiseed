using Newtonsoft.Json;

namespace Apps.Voiseed.Models.Batch
{
    public class UploadUrlResponse
    {
        [JsonProperty("uploadUrl")] public string UploadUrl { get; set; }
        [JsonProperty("fileUrl")] public string FileUrl { get; set; }
    }
}
