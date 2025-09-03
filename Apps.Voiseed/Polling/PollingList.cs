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

        [PollingEvent("On convert text to speech completed", Description = "Triggers when convert text to speech request completed")]
        public async Task<PollingEventResponse<DateMemory, TextToSpeechStatusResponse>> OnConvertTextToSpeechCompleted(
               PollingEventRequest<DateMemory> request, [PollingEventParameter][Display("Request ID")] string requestId)
        {
            var client = new VoiseedClient(InvocationContext.AuthenticationCredentialsProviders);

            var req = new RestRequest($"/inference/{requestId}/status", Method.Get);
            var details = await client.ExecuteWithErrorHandling<TextToSpeechStatusResponse>(req);

            var nowSuccess = string.Equals(details.Status, "SUCCESS", StringComparison.OrdinalIgnoreCase);

            if (request.Memory is null)
            {
                var freshMemory = new DateMemory
                {
                    LastInteractionDate = DateTime.UtcNow,
                    LastStatus = details.Status
                };

                if (nowSuccess)
                {
                    return new PollingEventResponse<DateMemory, TextToSpeechStatusResponse>
                    {
                        FlyBird = true,
                        Memory = freshMemory,
                        Result = details
                    };
                }

                return new PollingEventResponse<DateMemory, TextToSpeechStatusResponse>
                {
                    FlyBird = false,
                    Memory = freshMemory
                };
            }

            var memory = request.Memory;
            var wasSuccess = string.Equals(memory.LastStatus, "SUCCESS", StringComparison.OrdinalIgnoreCase);

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
