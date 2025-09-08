using Apps.Voiseed.Api;
using Apps.Voiseed.Models.Speech;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Utilities;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.Voiseed.Actions
{
    [ActionList("Speech")]
    public class SpeechActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : Invocable(invocationContext)
    {
        //[Action("Convert text to speech", Description = "Convert provided text to a speech with selected settings")]
        //public async Task<TextToSpeechResponse> ConvertTextToSpeech([ActionParameter] TextToSpeechRequest input)
        //{
        //    var texts = (input.Text ?? Enumerable.Empty<string>())
        //       .Where(t => !string.IsNullOrWhiteSpace(t))
        //       .ToList();

        //    if (texts.Count == 0)
        //        throw new PluginApplicationException("Text must contain at least one non-empty string.");

        //    var styles = (input.Styles ?? Array.Empty<string>())
        //        .Where(s => !string.IsNullOrWhiteSpace(s))
        //        .Distinct()
        //        .ToArray();

        //    if (styles.Length == 0)
        //        styles = new[] { "narration-normal" };

        //    const string ModelName = "xpressive";

        //    var body = new Dictionary<string, object>
        //    {
        //        ["texts"] = texts,
        //        ["styles"] = styles,
        //        ["model"] = ModelName,
        //        ["languageId"] = input.LanguageId,
        //        ["voice"] = input.Voice
        //    };

        //    if (input.GlossaryIds?.Any() == true)
        //        body["glossaryIds"] = input.GlossaryIds;

        //    if (!string.IsNullOrWhiteSpace(input.ExternalRequestId))
        //        body["externalRequestId"] = input.ExternalRequestId;

        //    var adv = new Dictionary<string, object>();
        //    if (input.Seed.HasValue) adv["seed"] = input.Seed;
        //    if (input.Diversity.HasValue) adv["diversity"] = input.Diversity;
        //    if (input.Expressivity.HasValue) adv["expressivity"] = input.Expressivity;

        //    var aq = new Dictionary<string, object>();
        //    if (input.OutputBitrate.HasValue) aq["outputBitrate"] = input.OutputBitrate;
        //    if (input.OutputSamplingRate.HasValue) aq["outputSamplingRate"] = input.OutputSamplingRate;
        //    if (aq.Count > 0) adv["audioQuality"] = aq;

        //    if (adv.Count > 0)
        //        body["advancedSettings"] = adv;

        //    var payload = Newtonsoft.Json.JsonConvert.SerializeObject(
        //        body,
        //        new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }
        //    );

        //    var req = new RestRequest("/inference", Method.Post).AddStringBody(payload, DataFormat.Json);
        //    return await Client.ExecuteWithErrorHandling<TextToSpeechResponse>(req);
        //}

        //[Action("Get convert text to speech status", Description = "Get convert text to speech status")]
        //public async Task<TextToSpeechStatusResponse> GetConvertTextToSpeechStatus([ActionParameter][Display("Request ID")] string requestId)
        //{
        //    var client = new VoiseedClient(invocationContext.AuthenticationCredentialsProviders);
        //    var req = new RestRequest($"/inference/{requestId}/status", Method.Get);
        //    return await client.ExecuteWithErrorHandling<TextToSpeechStatusResponse>(req);
        //}


        //[Action("Download TTS audio", Description = "Download converted text to speech audio files")]
        //public async Task<FileResponse> DownloadConvertTextToSpeechAudio([ActionParameter] TextToSpeechDownloadInput input)
        //{
        //    var client = new VoiseedClient(invocationContext.AuthenticationCredentialsProviders);

        //    var allUrls = new List<string>();
        //    var page = 1;
        //    while (true)
        //    {
        //        var req = new RestRequest($"/inference/{input.RequestId}/download", Method.Get)
        //            .AddParameter("page", page, ParameterType.QueryString);

        //        var pageData = await client.ExecuteWithErrorHandling<TextToSpeechUrls>(req);
        //        var urls = pageData?.Urls ?? new List<string>();
        //        if (urls.Count == 0) break;

        //        allUrls.AddRange(urls);
        //        page++;
        //    }

        //    if (allUrls.Count == 0)
        //        return new FileResponse { Files = Array.Empty<FileReference>() };

        //    var fileRefs = new List<FileReference>();
        //    for (int i = 0; i < allUrls.Count; i++)
        //    {
        //        var url = allUrls[i];
        //        var blk = await FileDownloader.DownloadFileBytes(url);

        //        var (finalName, finalContentType) = NormalizeAsWav(blk.Name, blk.ContentType, i);

        //        var fr = await fileManagementClient.UploadAsync(blk.FileStream, finalContentType, finalName);
        //        fileRefs.Add(fr);
        //    }

        //    return new FileResponse { Files = fileRefs };
        //}


        [Action("Convert text to speech", Description = "Convert provided text to a speech with selected settings")]
        public async Task<FileResponse> ConvertWaitAndDownload(
            [ActionParameter] TextToSpeechRequest input)
        {
            var client = new VoiseedClient(invocationContext.AuthenticationCredentialsProviders);

            var text = (input.Text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(text))
                throw new PluginApplicationException("Text must contain a non-empty string.");

            var style = (input.Styles ?? Enumerable.Empty<string>())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .ToList();

            string finalStyle;
            if (style.Count == 0) finalStyle = "narration-normal";
            else if (style.Count == 1) finalStyle = style[0];
            else throw new PluginMisconfigurationException($"Only one style is supported for a single text. Received: {style.Count}.");

            if (string.IsNullOrWhiteSpace(input.Voice))
                throw new PluginMisconfigurationException("Voice is required.");

            const string ModelName = "xpressive";

            var body = new Dictionary<string, object>
            {
                ["texts"] = new[] { text },
                ["styles"] = new[] { finalStyle },
                ["model"] = ModelName,
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

            if (adv.Count > 0)
                body["advancedSettings"] = adv;

            var payload = Newtonsoft.Json.JsonConvert.SerializeObject(
                body,
                new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore }
            );

            var createReq = new RestRequest("/inference", Method.Post).AddStringBody(payload, DataFormat.Json);
            var created = await client.ExecuteWithErrorHandling<Apps.Voiseed.Models.Speech.TextToSpeechResponse>(createReq);
            var requestId = created?.RequestId ?? throw new PluginApplicationException("Inference created but no 'requestId' in response.");

            var delay = TimeSpan.FromMilliseconds(800);
            while (true)
            {
                var stReq = new RestRequest($"/inference/{requestId}/status", Method.Get);
                var st = await client.ExecuteWithErrorHandling<Apps.Voiseed.Models.Speech.TextToSpeechStatusResponse>(stReq);

                var raw = st?.Status ?? string.Empty;
                var status = raw.Trim().ToLowerInvariant();

                if (status == "success") break;
                if (status == "failure" || status == "failed" || status == "error")
                    throw new PluginApplicationException($"Inference '{requestId}' finished with error status '{raw}'.");

                await Task.Delay(delay);
                var nextMs = Math.Min(delay.TotalMilliseconds * 1.8 + Random.Shared.Next(0, 250), 5000);
                delay = TimeSpan.FromMilliseconds(nextMs);
            }

            string? firstUrl = null;
            var page = 1;
            while (firstUrl == null)
            {
                var dlReq = new RestRequest($"/inference/{requestId}/download", Method.Get)
                    .AddParameter("page", page, ParameterType.QueryString);

                var pageObj = await client.ExecuteWithErrorHandling<Newtonsoft.Json.Linq.JObject>(dlReq);
                var urls = pageObj.SelectToken("urls")?.ToObject<List<string>>() ?? new List<string>();

                if (urls.Count > 0)
                    firstUrl = urls[0];
                else if (page > 3)
                    break;
                else
                    page++;
            }

            if (string.IsNullOrWhiteSpace(firstUrl))
                throw new PluginApplicationException("No audio URL was returned for the inference.");

            var blob = await FileDownloader.DownloadFileBytes(firstUrl);
            var (finalName, finalContentType) = NormalizeAsWav(blob.Name, blob.ContentType, 0);
            var fr = await fileManagementClient.UploadAsync(blob.FileStream, finalContentType, finalName);

            return new FileResponse { File = fr };
        }


        public (string name, string contentType) NormalizeAsWav(string? originalName, string? originalCt, int index)
        {
            string name = string.IsNullOrWhiteSpace(originalName)
                ? $"voiseed_line_{index:000}.wav"
                : originalName!;

            string ext = System.IO.Path.GetExtension(name);
            string ct = originalCt ?? "";

            if (string.IsNullOrWhiteSpace(ext))
            {
                name = $"{System.IO.Path.GetFileNameWithoutExtension(name)}.wav";
                return (name, "audio/wav");
            }

            if (ext.Equals(".wav", StringComparison.OrdinalIgnoreCase))
                return (name, "audio/wav");

            if (ct.Equals("application/octet-stream", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(ct))
            {
                var mapped = MapContentTypeByExt(ext);
                if (!string.IsNullOrEmpty(mapped)) ct = mapped;
            }
            return (name, string.IsNullOrWhiteSpace(ct) ? "application/octet-stream" : ct);
        }

        public string? MapContentTypeByExt(string ext)
        {
            switch (ext.ToLowerInvariant())
            {
                case ".wav": return "audio/wav";
                case ".mp3": return "audio/mpeg";
                case ".ogg": return "audio/ogg";
                case ".m4a": return "audio/mp4";
                case ".flac": return "audio/flac";
                default: return null;
            }
        }
    }
}
