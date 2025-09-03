using Apps.Voiseed.Api;
using Apps.Voiseed.Models.Speech;
using Apps.Voiseed.Polling.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using RestSharp;

namespace Apps.Voiseed.Polling
{
    [PollingEventList]
    public class PollingList : Invocable
    {
        public PollingList(InvocationContext invocationContext) : base(invocationContext)
        {
        }

        [PollingEvent("On convert text to speech completed", Description = "Triggers when a TTS inference request completed")]
        public async Task<PollingEventResponse<DateMemory, TextToSpeechStatusResponse>> OnConvertTextToSpeechCompleted(
               PollingEventRequest<DateMemory> request, [PollingEventParameter][Display("Request ID")] string requestId)
        {
            var client = new VoiseedClient(InvocationContext.AuthenticationCredentialsProviders);

            var req = new RestRequest($"/inference/{requestId}/status", Method.Get);
            var details = await client.ExecuteWithErrorHandling<TextToSpeechStatusResponse>(req);

            if (request.Memory is null)
            {
                return new PollingEventResponse<DateMemory, TextToSpeechStatusResponse>
                {
                    FlyBird = false,
                    Memory = new DateMemory
                    {
                        LastInteractionDate = DateTime.UtcNow,
                        LastStatus = details.Status
                    }
                };
            }

            var memory = request.Memory;
            var wasSuccess = string.Equals(memory.LastStatus, "SUCCESS", StringComparison.OrdinalIgnoreCase);
            var nowSuccess = string.Equals(details.Status, "SUCCESS", StringComparison.OrdinalIgnoreCase);

            memory.LastStatus = details.Status;
            memory.LastInteractionDate = DateTime.UtcNow;

            if (!wasSuccess && nowSuccess)
            {
                return new PollingEventResponse<DateMemory, TextToSpeechStatusResponse>
                {
                    FlyBird = true,
                    Memory = memory,
                    Result = details
                };
            }

            return new PollingEventResponse<DateMemory, TextToSpeechStatusResponse>
            {
                FlyBird = false,
                Memory = memory
            };
        }
    }
}
