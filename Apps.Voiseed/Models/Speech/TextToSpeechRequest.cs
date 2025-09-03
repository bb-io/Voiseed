using Apps.Voiseed.DataHandlers;
using Apps.Voiseed.Handlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Voiseed.Models.Speech
{
    public class TextToSpeechRequest
    {
        public IEnumerable<string> Text { get; set; }

        [Display("Language")]
        [DataSource(typeof(LanguagesDataHandler))]
        public string LanguageId { get; set; }

        [Display("Voice")]
        [DataSource(typeof(VoicesDataHandler))]
        public string Voice { get; set; }

        [Display("Styles")]

        public IEnumerable<string> Styles { get; set; } = Array.Empty<string>();

        [Display("Glossary IDs")]
        [DataSource(typeof(GlossariesDataHandler))]
        public IEnumerable<string>? GlossaryIds { get; set; }

        [Display("External request ID")]
        public string? ExternalRequestId { get; set; }

        public int? Seed { get; set; }
        public double? Diversity { get; set; }
        public double? Expressivity { get; set; }

        [Display("Output bitrate")]
        public int? OutputBitrate { get; set; }

        [Display("Output sampling rate")]
        public int? OutputSamplingRate { get; set; }
    }
}
