using Apps.Voiseed.DataHandlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Voiseed.Models.Batch
{
    public class CreateBatchRequest
    {
        [Display("Project ID")]
        public string ProjectId { get; set; }

        [Display("Language")]
        [StaticDataSource(typeof(BatchLanguagesDataHandler))]
        public string LanguageId { get; set; }

        [Display("Script URL (http/https)")]
        public string? ScriptUrl { get; set; }

        [Display("Include ID column?")]
        public bool IncludeId { get; set; }

        [Display("Include Character column?")]
        public bool IncludeCharacter { get; set; }

        [Display("Include Emotion column?")]
        public bool IncludeEmotion { get; set; }

        [Display("Include Intensity column?")]
        public bool IncludeIntensity { get; set; }

        [Display("IDs (optional)")]
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
        public IEnumerable<string> Texts { get; set; }

        [Display("Batch name")]
        public string? Name { get; set; }

        [Display("Automatic inference")]
        public bool AutomaticInference { get; set; }

        [Display("Alternative takes")]
        public int? NoOfAlternativeTakes { get; set; }
    }
}
