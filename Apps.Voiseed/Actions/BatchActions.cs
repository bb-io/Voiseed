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
        [Action("Create batch", Description = "Builds an XLSX from selected columns + parallel arrays, uploads it, and creates a batch")]
        public async Task<BatchResponse> CreateBatch([ActionParameter] CreateBatchRequest input)
        {
            var columns = new List<string> { BatchExcelBuilder.COL_TEXT };
            if (input.IncludeId) columns.Insert(0, BatchExcelBuilder.COL_ID);
            if (input.IncludeCharacter) columns.Insert(columns.IndexOf(BatchExcelBuilder.COL_TEXT), BatchExcelBuilder.COL_CHARACTER);
            if (input.IncludeEmotion) columns.Insert(columns.IndexOf(BatchExcelBuilder.COL_TEXT), BatchExcelBuilder.COL_EMOTION);
            if (input.IncludeIntensity) columns.Insert(columns.IndexOf(BatchExcelBuilder.COL_TEXT), BatchExcelBuilder.COL_INTENSITY);

            var xlsx = BatchExcelBuilder.BuildXlsx(
                columns,
                input.IncludeId ? input.Ids : null,
                input.IncludeCharacter ? input.Characters : null,
                input.IncludeEmotion ? input.Emotions : null,
                input.IncludeIntensity ? input.Intensities : null,
                input.Texts,
                input.LanguageId
            );

            var fileName = $"script_{input.LanguageId}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx";
            const string excelCt = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            xlsx.Position = 0;
            FileReference scriptRef = await fileManagementClient.UploadAsync(xlsx, excelCt, fileName);

            if (string.IsNullOrWhiteSpace(scriptRef.Url))
                throw new PluginApplicationException("Uploaded script file does not have a public URL. Configure your storage to return FileReference.Url.");

            var client = new VoiseedClient(invocationContext.AuthenticationCredentialsProviders);
            var body = new
            {
                name = string.IsNullOrWhiteSpace(input.Name)
                    ? $"Batch_{input.LanguageId}_{DateTime.UtcNow:yyyyMMdd}"
                    : input.Name,
                scriptPath = scriptRef.Url,
                settings = new
                {
                    automaticInference = input.AutomaticInference,
                    noOfAlternativeTakes = input.NoOfAlternativeTakes ?? 0
                }
            };

            var req = new RestRequest($"/projects/{input.ProjectId}/batches", Method.Post).AddJsonBody(body);
            var batch = await client.ExecuteWithErrorHandling<BatchDto>(req);

            return new BatchResponse { Batch = batch, ScriptFile = scriptRef };
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
