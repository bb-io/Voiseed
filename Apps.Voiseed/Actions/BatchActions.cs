using Apps.Voiseed.Api;
using Apps.Voiseed.Models.Batch;
using Apps.Voiseed.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.Voiseed.Actions
{
    [ActionList("Batch")]
    public class BatchActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : Invocable(invocationContext)
    {
        [Action("Create batch", Description = "Creates a batch")]
        public async Task<BatchResponse> CreateBatch([ActionParameter] CreateBatchRequest input)
        {
            string? scriptPath = null;
            FileReference? scriptRef = null;
            const string excelCt = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            if (input.FileScript != null)
            {
                if (!string.IsNullOrWhiteSpace(input.FileScript.Url))
                {
                    scriptRef = input.FileScript;
                    scriptPath = input.FileScript.Url!.Trim();
                }
                else
                {
                    using var stream = await fileManagementClient.DownloadAsync(input.FileScript);
                    stream.Position = 0;

                    var ct = string.IsNullOrWhiteSpace(input.FileScript.ContentType) ? excelCt : input.FileScript.ContentType!;
                    var name = string.IsNullOrWhiteSpace(input.FileScript.Name)
                        ? $"script_{input.LanguageId}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx"
                        : input.FileScript.Name!;

                    var reuploaded = await fileManagementClient.UploadAsync(stream, ct, name);
                    scriptRef = reuploaded;
                    scriptPath = reuploaded.Url ?? throw new PluginApplicationException("Unable to obtain URL for the provided script file.");
                }
            }

            if (string.IsNullOrWhiteSpace(scriptPath))
            {
                var texts = input.Texts?.ToList() ?? new();
                if (texts.Count == 0)
                    throw new PluginApplicationException("Texts must not be empty.");

                bool haveCharacters = input.Characters?.Any() == true;
                bool haveEmotions = input.Emotions?.Any() == true;
                bool haveIntensity = input.Intensities?.Any() == true;
                bool haveIds = input.Ids?.Any() == true;

                if (haveCharacters && input.Characters!.Count() != texts.Count)
                    throw new PluginApplicationException($"Characters length ({input.Characters!.Count()}) must equal Texts length ({texts.Count}).");
                if (haveEmotions && input.Emotions!.Count() != texts.Count)
                    throw new PluginApplicationException($"Emotions length ({input.Emotions!.Count()}) must equal Texts length ({texts.Count}).");
                if (haveIntensity && input.Intensities!.Count() != texts.Count)
                    throw new PluginApplicationException($"Intensities length ({input.Intensities!.Count()}) must equal Texts length ({texts.Count}).");
                if (haveIds && input.Ids!.Count() != texts.Count)
                    throw new PluginApplicationException($"IDs length ({input.Ids!.Count()}) must equal Texts length ({texts.Count}).");

                var ids = haveIds ? input.Ids! : Enumerable.Range(1, texts.Count);

                var columns = new List<string> { BatchExcelBuilder.COL_ID };
                if (haveCharacters) columns.Add(BatchExcelBuilder.COL_CHARACTER);
                if (haveEmotions) columns.Add(BatchExcelBuilder.COL_EMOTION);
                if (haveIntensity) columns.Add(BatchExcelBuilder.COL_INTENSITY);
                columns.Add(BatchExcelBuilder.COL_TEXT);

                var xlsx = BatchExcelBuilder.BuildXlsx(
                    columns,
                    ids,
                    haveCharacters ? input.Characters : null,
                    haveEmotions ? input.Emotions : null,
                    haveIntensity ? input.Intensities : null,
                    texts,
                    input.LanguageId
                );

                var fileName = $"script_{input.LanguageId}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx";
                xlsx.Position = 0;
                scriptRef = await fileManagementClient.UploadAsync(xlsx, excelCt, fileName);
                scriptPath = scriptRef.Url!;
            }

            var commonName = string.IsNullOrWhiteSpace(input.Name)
                ? $"Batch_{input.LanguageId}_{DateTime.UtcNow:yyyyMMdd}"
                : input.Name;

            var settings = new
            {
                automaticInference = input.AutomaticInference,
                noOfAlternativeTakes = input.NoOfAlternativeTakes ?? 0
            };

            var bulkBody = new
            {
                    batches = new[]
                    {
                    new {
                        projectId = input.ProjectId,
                        name = commonName,
                        scriptPath = scriptPath,
                        settings = settings
                    }
                    }
            };

            try
            {
                var bulkReq = new RestRequest("/batches/bulk", Method.Post).AddJsonBody(bulkBody);
                var j = await Client.ExecuteWithErrorHandling<Newtonsoft.Json.Linq.JObject>(bulkReq);

                var bulkRequestId =
                    (string?)j.SelectToken("batchRequestId") ??
                    (string?)j.SelectToken("id") ??
                    (string?)j.SelectToken("requestId");

                if (string.IsNullOrWhiteSpace(bulkRequestId))
                    throw new PluginApplicationException("Bulk create succeeded but no 'batchRequestId' was found in response.");

                return new BatchResponse
                {
                    Batch = null,
                    ScriptFile = scriptRef,
                    BulkRequestId = bulkRequestId
                };
            }
            catch (PluginApplicationException ex)
            {
                var msg = ex.Message ?? string.Empty;
                if (msg.Contains("Authorization header requires 'Credential'", StringComparison.OrdinalIgnoreCase) ||
                    msg.Contains("SignedHeaders", StringComparison.OrdinalIgnoreCase) ||
                    msg.Contains("Signature", StringComparison.OrdinalIgnoreCase))
                {
                    throw new PluginMisconfigurationException(
                        "Bulk endpoint requires AWS Signature Version 4 (service: execute-api, правильний region). " +
                        "Налаштуйте SigV4 облікові дані для Voiseed/Revoiceit.");
                }
                throw;
            }
        }

        [Action("Get batch", Description = "Get batch details/status by ID")]
        public async Task<BatchDetailsDto> GetBatch([ActionParameter][Display("Batch ID")] string batchId)
        {
            var client = new VoiseedClient(invocationContext.AuthenticationCredentialsProviders);
            var req = new RestRequest($"/batches/{batchId}", Method.Get);
            return await client.ExecuteWithErrorHandling<BatchDetailsDto>(req);
        }
    }
}
