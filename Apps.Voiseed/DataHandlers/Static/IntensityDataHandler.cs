using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Voiseed.DataHandlers.Static
{
    public class IntensityDataHandler : IStaticDataSourceItemHandler
    {
        public IEnumerable<DataSourceItem> GetData()
        {
            return new List<DataSourceItem>
            {
                new("Low", "Low"),
                new("Normal", "Normal"),
                new("High", "High"),
            };
        }
    }
}