using Apps.Voiseed.Actions;
using Tests.Voiseed.Base;

namespace Tests.Voiseed
{
    [TestClass]
    public class SpeechActionTests : TestBase
    {
        //[TestMethod]
        //public async Task ConvertTextToSpeech_IsSuccess()
        //{
        //    var action = new SpeechActions(InvocationContext, FileManager);
        //    var response = await action.ConvertTextToSpeech(new()
        //    {
        //        Text = new[] { "Hello, this is a test." },
        //        LanguageId = "fr-fr",
        //        Voice = "Laura",
        //    });
        //    var json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //    Console.WriteLine(json);
        //    Assert.IsNotNull(response);
        //    Assert.IsFalse(string.IsNullOrEmpty(response.RequestId));
        //}

        [TestMethod]
        public async Task ConvertTextToSpeech_IsSuccess()
        {
            var action = new SpeechActions(InvocationContext, FileManager);
            var response = await action.ConvertWaitAndDownload(new()
            {
                Text = "Hello, this is a test. with united actions",
                LanguageId = "fr-fr",
                Voice = "Laura",
            });
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            Console.WriteLine(json);
            Assert.IsNotNull(response);
        }

        //[TestMethod]
        //public async Task GetConvertTextToSpeechStatus_IsSuccess()
        //{
        //    var action = new SpeechActions(InvocationContext, FileManager);
        //    var response = await action.GetConvertTextToSpeechStatus("4f3adfc0-d0a7-4307-821a-f91d0e89dfa2");

        //    var json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //    Console.WriteLine(json);
        //    Assert.IsNotNull(response);
        //}

        //[TestMethod]
        //public async Task DownloadConvertTextToSpeechAudio_IsSuccess()
        //{
        //    var action = new SpeechActions(InvocationContext, FileManager);
        //    var response = await action.DownloadConvertTextToSpeechAudio(new Apps.Voiseed.Models.Speech.TextToSpeechDownloadInput { RequestId= "1cb13d69-f4b7-495a-89fa-dcf0035b4b69" });

        //    var json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
        //    Console.WriteLine(json);
        //    Assert.IsNotNull(response);
        //}
    }
}
