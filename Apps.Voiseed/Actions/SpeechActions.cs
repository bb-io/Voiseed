using Apps.Voiseed.Api;
using Apps.Voiseed.Models.Speech;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.Voiseed.Actions
{
    [ActionList]
    public class SpeechActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : Invocable(invocationContext)
    {
        [Action("Convert text to speech", Description = "Convert provided text to a speech with selected settings")]
        public async Task<TextToSpeechResponse> ConvertTextToSpeech([ActionParameter] TextToSpeechRequest input)
        {
            var styles = (input.Styles ?? Array.Empty<string>()).ToArray();
            if (styles.Length == 0) styles = new[] { "neutral" };

            var body = new Dictionary<string, object>
            {
                ["texts"] = input.Text,
                ["styles"] = styles,
                ["model"] = input.Model,
                ["languageId"] = input.LanguageId,
                ["voice"] = input.Voice
            };

            if (input.GlossaryIds?.Any() == true) body["glossaryIds"] = input.GlossaryIds;
            if (!string.IsNullOrWhiteSpace(input.ExternalRequestId)) body["externalRequestId"] = input.ExternalRequestId;

            var adv = new Dictionary<string, object>();
            if (input.Seed.HasValue) adv["seed"] = input.Seed;
            if (input.Diversity.HasValue) adv["diversity"] = input.Diversity;
            if (input.Expressivity.HasValue) adv["expressivity"] = input.Expressivity;

            var aq = new Dictionary<string, object>();
            if (input.OutputBitrate.HasValue) aq["outputBitrate"] = input.OutputBitrate;
            if (input.OutputSamplingRate.HasValue) aq["outputSamplingRate"] = input.OutputSamplingRate;
            if (aq.Count > 0) adv["audioQuality"] = aq;

            if (adv.Count > 0) body["advancedSettings"] = adv;

            var payload = Newtonsoft.Json.JsonConvert.SerializeObject(
                body,
                new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }
            );

            var req = new RestRequest("/inference", Method.Post).AddStringBody(payload, DataFormat.Json);
            return await Client.ExecuteWithErrorHandling<TextToSpeechResponse>(req);
        }

        [Action("Get convert text to speech status", Description = "Get convert text to speech status")]
        public async Task<TextToSpeechStatusResponse> GetConvertTextToSpeechStatus([ActionParameter] string requestId)
        {
            var client = new VoiseedClient(invocationContext.AuthenticationCredentialsProviders);
            var req = new RestRequest($"/inference/{requestId}/status", Method.Get);
            return await client.ExecuteWithErrorHandling<TextToSpeechStatusResponse>(req);
        }


        [Action("Download inference audio", Description = "Download a single line as an audio file")]
        public async Task<FileResponse> DownloadConvertTextToSpeechAudio([ActionParameter] TextToSpeechDownloadInput input)
        {
            var client = new VoiseedClient(invocationContext.AuthenticationCredentialsProviders);

            var req = new RestRequest($"/inference/{input.RequestId}/download", Method.Get)
                .AddQueryParameter("return_type", "audio");

            var resp = await client.ExecuteWithErrorHandling(req);
            var bytes = resp.RawBytes ?? Array.Empty<byte>();
            var contentType = string.IsNullOrWhiteSpace(resp.ContentType) ? "audio/wav" : resp.ContentType!;
            var ext =
                contentType.Contains("mp3", StringComparison.OrdinalIgnoreCase) ? "mp3" :
                contentType.Contains("ogg", StringComparison.OrdinalIgnoreCase) ? "ogg" :
                contentType.Contains("mpeg", StringComparison.OrdinalIgnoreCase) ? "mp3" :
                contentType.Contains("m4a", StringComparison.OrdinalIgnoreCase) ? "m4a" : "wav";

            var fileName = $"voiseed_{input.RequestId}.{ext}";

            using var ms = new MemoryStream(bytes);
            var fileRef = await fileManagementClient.UploadAsync(ms, contentType, fileName);
            return new FileResponse { File = fileRef };
        }

    }
}
