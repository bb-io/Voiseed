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
            var response = await action.ConvertTextToSpeech(new()
            {
                Text = new[] { "Hello, this is a test." },
                Model = "xpressive",
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
            var response = await action.GetConvertTextToSpeechStatus("9e08a0b4-2328-4e16-9012-4280ffba783b");

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
