using Apps.Voiseed.DataHandlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Voiseed.Models.Batch
{
    public class CreateBatchRequest
    {
        [Display("Project ID")]
        public string ProjectId { get; set; }

        [Display("Language")]
        [StaticDataSource(typeof(BatchLanguagesDataHandler))]
        public string? LanguageId { get; set; }

        [Display("Script file")]
        public FileReference? FileScript { get; set; }

        [Display("IDs")]
        public IEnumerable<int>? Ids { get; set; }

        [Display("Characters")]
        public IEnumerable<string>? Characters { get; set; }

        [Display("Emotions")]
        [StaticDataSource(typeof(EmotionDataHandler))]
        public IEnumerable<string>? Emotions { get; set; }

        [Display("Intensity")]
        [StaticDataSource(typeof(IntensityDataHandler))]
        public IEnumerable<string>? Intensities { get; set; }

        [Display("Texts")]
        public IEnumerable<string>? Texts { get; set; }

        [Display("Batch name")]
        public string? Name { get; set; }

        [Display("Automatic inference")]
        public bool AutomaticInference { get; set; } = true;

        [Display("Alternative takes")]
        public int? NoOfAlternativeTakes { get; set; }
    }
}
