using Apps.Voiseed.Models;
using Apps.Voiseed.Models.Speech;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Voiseed.DataHandlers
{
    public class VoiceStylesDataHandler(InvocationContext invocationContext) : Invocable(invocationContext), IAsyncDataSourceItemHandler
    {
        public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"/models/xpressive/styles", Method.Get);
            var voices = await Client.ExecuteWithErrorHandling<List<VoiceStyleDto>>(request);

            return voices.Select(voice => new DataSourceItem(voice.Style, voice.Style));
        }
    }
}

