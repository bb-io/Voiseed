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
    }
}
