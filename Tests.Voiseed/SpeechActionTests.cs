using Apps.Voiseed.Actions;
using Tests.Voiseed.Base;

namespace Tests.Voiseed
{
    [TestClass]
    public class SpeechActionTests : TestBase
    {
        [TestMethod]
        public async Task ConvertTextToSpeech_IsSuccess()
        {
            var action = new SpeechActions(InvocationContext, FileManager);
            var response = await action.ConvertTextToSpeech(new Apps.Voiseed.Models.Speech.ModelRequest { Model = "xpressive"},new()
            {
                Text = new[] { "Hello, this is a test." },
                LanguageId = "en-us",
                Voice = "Laura",
                Styles = new[] { "narration-normal" },
            });
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            Console.WriteLine(json);
            Assert.IsNotNull(response);
            Assert.IsFalse(string.IsNullOrEmpty(response.RequestId));
        }

        [TestMethod]
        public async Task GetConvertTextToSpeechStatus_IsSuccess()
        {
            var action = new SpeechActions(InvocationContext, FileManager);
            var response = await action.GetConvertTextToSpeechStatus("8db6406f-a1e2-4316-bc1a-4d478621eef7");

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            Console.WriteLine(json);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task DownloadConvertTextToSpeechAudio_IsSuccess()
        {
            var action = new SpeechActions(InvocationContext, FileManager);
            var response = await action.DownloadConvertTextToSpeechAudio(new Apps.Voiseed.Models.Speech.TextToSpeechDownloadInput { RequestId= "9e08a0b4-2328-4e16-9012-4280ffba783b" });

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            Console.WriteLine(json);
            Assert.IsNotNull(response);
        }
    }
}
