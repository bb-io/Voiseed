using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Voiseed.Handlers.Static;
public class ProjectTypeHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData()
    {
        return new List<DataSourceItem>
        {
            new("Line dubbing", "line-dubbing"),
            new("Media dubbing", "media-dubbing"),
            new("Audio production", "audio-production"),
        };
    }
}
