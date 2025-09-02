using Apps.Voiseed.Models;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Voiseed.DataHandlers
{
    public class ModelsDataHandler(InvocationContext invocationContext) : Invocable(invocationContext), IAsyncDataSourceItemHandler
    {
        public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
        {
            var request = new RestRequest("/models", Method.Get);
            var voices = await Client.ExecuteWithErrorHandling<List<ModelDto>>(request);

            return voices.Select(voice => new DataSourceItem(voice.Model, voice.Model));
        }
    }
}
