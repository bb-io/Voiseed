using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Voiseed.DataHandlers.Static
{
    public class EmotionDataHandler : IStaticDataSourceItemHandler
    {
        public IEnumerable<DataSourceItem> GetData()
        {
            return new List<DataSourceItem>
            {
                new("Anger", "Anger"),
                new("Curiosity", "Curiosity"),
                new("Fear", "Fear"),
                new("Joy", "Joy"),
                new("Narration", "Narration"),
                new("Pain", "Pain"),
                new("Pleasure", "Pleasure"),
                new("Sadness", "Sadness"),
                new("Surprise", "Surprise"),
            };
        }
    }
}