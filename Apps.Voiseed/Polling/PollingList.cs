using Apps.Voiseed.Api;
using Apps.Voiseed.Models.Batch;
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

        [PollingEvent("On batch completed", Description = "Triggers when a batch reaches a completed state")]
        public async Task<PollingEventResponse<DateMemory, BatchDetailsDto>> OnBatchCompleted(
           PollingEventRequest<DateMemory> request,
           [PollingEventParameter][Display("Batch ID")] string batchId)
        {
            var client = new VoiseedClient(InvocationContext.AuthenticationCredentialsProviders);
            var req = new RestRequest($"/batches/{batchId}", Method.Get);
            var details = await client.ExecuteWithErrorHandling<BatchDetailsDto>(req);

            var normalized = NormalizeOverallStatus(details);
            bool nowCompleted = string.Equals(normalized, "COMPLETED", StringComparison.OrdinalIgnoreCase);

            if (request.Memory is null)
            {
                var memory = new DateMemory
                {
                    LastInteractionDate = DateTime.UtcNow,
                    LastStatus = normalized
                };

                return new PollingEventResponse<DateMemory, BatchDetailsDto>
                {
                    FlyBird = nowCompleted,
                    Memory = memory,
                    Result = nowCompleted ? details : null
                };
            }

            var mem = request.Memory;
            bool wasCompleted = string.Equals(mem.LastStatus, "COMPLETED", StringComparison.OrdinalIgnoreCase);

            mem.LastStatus = normalized;
            mem.LastInteractionDate = DateTime.UtcNow;

            if (!wasCompleted && nowCompleted)
            {
                return new PollingEventResponse<DateMemory, BatchDetailsDto>
                {
                    FlyBird = true,
                    Memory = mem,
                    Result = details
                };
            }

            return new PollingEventResponse<DateMemory, BatchDetailsDto>
            {
                FlyBird = false,
                Memory = mem
            };
        }

        private static string NormalizeOverallStatus(BatchDetailsDto d)
        {
            var raw = (d?.Status ?? "").Trim();
            var s = raw.ToUpperInvariant();
 
            if (s is "COMPLETED" or "SUCCESS" or "DONE" or "READY" or "DUBBED")
                return "COMPLETED";
            if (s is "FAILED" or "ERROR" or "ABORTED" or "CANCELLED")
                return "FAILED";

            if ((d?.CompletionPercentage ?? 0) >= 100) return "COMPLETED";
            if (d?.Translations is { Count: > 0 } &&
                d.Translations.All(t => t.IsCompleted || string.Equals(t.Status, "completed", StringComparison.OrdinalIgnoreCase)))
                return "COMPLETED";

            return string.IsNullOrEmpty(raw) ? "UNKNOWN" : s;
        }
    }
}
