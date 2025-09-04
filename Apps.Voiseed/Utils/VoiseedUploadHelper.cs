using Apps.Voiseed.Api;
using Apps.Voiseed.Models.Batch;
using Blackbird.Applications.Sdk.Common.Exceptions;
using RestSharp;

namespace Apps.Voiseed.Utils
{
    public static class VoiseedUploadHelper
    {
        public static async Task<string> UploadScriptAndGetUrl(
            VoiseedClient client,
            Stream stream,
            string fileName,
            string contentType)
        {
            var upReq = new RestRequest("/media/upload-url", Method.Post)
                .AddJsonBody(new { fileName, contentType });

            var up = await client.ExecuteWithErrorHandling<UploadUrlResponse>(upReq);
            if (string.IsNullOrWhiteSpace(up?.UploadUrl) || string.IsNullOrWhiteSpace(up?.FileUrl))
                throw new PluginApplicationException("Voiseed did not return a valid uploadUrl/fileUrl.");

            byte[] bytes;
            if (stream is MemoryStream ms) bytes = ms.ToArray();
            else { using var tmp = new MemoryStream(); await stream.CopyToAsync(tmp); bytes = tmp.ToArray(); }

            var putClient = new RestClient();
            var putReq = new RestRequest(up.UploadUrl, Method.Put);
            putReq.AddHeader("Content-Type", contentType);
            putReq.AddParameter(contentType, bytes, ParameterType.RequestBody);

            var putRes = await putClient.ExecuteAsync(putReq);
            if (!putRes.IsSuccessStatusCode)
                throw new PluginApplicationException($"Upload to presigned URL failed: {(int)putRes.StatusCode} {putRes.StatusDescription}");

            return up.FileUrl;
        }
    }
}
