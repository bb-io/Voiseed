using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Voiseed.Handlers.Static;
public class ProjectTypeHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData()
    {
        return new List<DataSourceItem>
        {
            new("line-dubbing", "Line dubbing"),
            new("media-dubbing", "Media dubbing"),
            new("audio-production", "Audio production"),
        };
    }
}
