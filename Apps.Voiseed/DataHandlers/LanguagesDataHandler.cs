using Apps.Voiseed.Models;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Voiseed.DataHandlers
{
    public class LanguagesDataHandler(InvocationContext invocationContext) : Invocable(invocationContext), IAsyncDataSourceItemHandler
    {
        public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
        {
            var request = new RestRequest("/languages", Method.Get);
            var voices = await Client.ExecuteWithErrorHandling<List<LanguageDto>>(request);

            return voices.Select(voice => new DataSourceItem(voice.Id, voice.Name));
        }
    }
}