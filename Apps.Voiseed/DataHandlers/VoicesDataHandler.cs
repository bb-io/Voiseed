using Apps.Voiseed.Models;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Voiseed.Handlers;
public class VoicesDataHandler(InvocationContext invocationContext) : Invocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new RestRequest("/voices", Method.Get);
        var voices = await Client.ExecuteWithErrorHandling<List<Voice>>(request);

        return voices.Select(voice => new DataSourceItem(voice.Name, voice.Name));
    }
}
