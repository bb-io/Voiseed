using Blackbird.Applications.Sdk.Common;

namespace Apps.Voiseed.Models.Batch
{
    public class BatchDownloadInput
    {
        [Display("Batch ID")]
        public string BatchId { get; set; }

        [Display("Project ID (optional)")]
        public string? ProjectId { get; set; }
    }

    public class BatchDownloadUrls
    {
        public List<string> Urls { get; set; } = new();
    }
}
