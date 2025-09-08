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

                    var ct = string.IsNullOrWhiteSpace(input.FileScript.ContentType)
                        ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                        : input.FileScript.ContentType!;

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
                const string excelCt = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

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

                var columns = new List<string> { BatchExcelBuilder.COL_ID };
                if (haveCharacters) columns.Add(BatchExcelBuilder.COL_CHARACTER);
                if (haveEmotions) columns.Add(BatchExcelBuilder.COL_EMOTION);
                if (haveIntensity) columns.Add(BatchExcelBuilder.COL_INTENSITY);
                columns.Add(BatchExcelBuilder.COL_TEXT);

                var xlsx = BatchExcelBuilder.BuildXlsx(
                    columns,
                    haveIds ? input.Ids : null,
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

            if (string.IsNullOrWhiteSpace(scriptPath))
                throw new PluginApplicationException("scriptPath is empty.");

            var nameValue = string.IsNullOrWhiteSpace(input.Name)
                ? $"Batch_{input.LanguageId}_{DateTime.UtcNow:yyyyMMdd}"
                : input.Name;

            var settings = new Newtonsoft.Json.Linq.JObject();

            settings["automaticInference"] = input.AutomaticInference;
                                                          
            if (input.NoOfAlternativeTakes.HasValue)
            {
                var n = Math.Max(0, Math.Min(2, input.NoOfAlternativeTakes.Value));
                settings["noOfAlternativeTakes"] = n;
            }

            var body = new Newtonsoft.Json.Linq.JObject
            {
                ["name"] = nameValue,
                ["scriptPath"] = scriptPath
            };

            if (settings.HasValues)
                body["settings"] = settings;

            var payload = Newtonsoft.Json.JsonConvert.SerializeObject(
                body,
                new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }
            );

            var req = new RestRequest($"/projects/{input.ProjectId}/batches", Method.Post)
                .AddStringBody(payload, DataFormat.Json);

            var batch = await Client.ExecuteWithErrorHandling<BatchDto>(req);

            return new BatchResponse { Batch = batch, ScriptFile = scriptRef! };
        }

        [Action("Get batch", Description = "Get batch details/status by ID")]
        public async Task<BatchDetailsDto> GetBatch([ActionParameter][Display("Batch ID")] string batchId)
        {
            var client = new VoiseedClient(invocationContext.AuthenticationCredentialsProviders);
            var req = new RestRequest($"/batches/{batchId}", Method.Get);
            return await client.ExecuteWithErrorHandling<BatchDetailsDto>(req);
        }

        private static bool IsHttpUrl(string? url)
           => Uri.TryCreate(url, UriKind.Absolute, out var u) &&
              (u.Scheme == Uri.UriSchemeHttp || u.Scheme == Uri.UriSchemeHttps);
    }
}
