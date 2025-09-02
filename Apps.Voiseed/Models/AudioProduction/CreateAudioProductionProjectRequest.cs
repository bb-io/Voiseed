using Apps.Voiseed.DataHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Voiseed.Models.AudioProduction
{
    public class CreateAudioProductionProjectRequest
    {
        [Display("Name")] public string Name { get; set; }
        [Display("Target language IDs")][DataSource(typeof(LanguagesDataHandler))] public IEnumerable<string> TargetLanguages { get; set; }
        [Display("Model")][DataSource(typeof(ModelsDataHandler))] public string Model { get; set; }
        [Display("Source language (optional)")][DataSource(typeof(LanguagesDataHandler))] public string? SourceLanguage { get; set; }
    }
}
