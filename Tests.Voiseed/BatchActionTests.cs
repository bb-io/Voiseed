using Apps.Voiseed.Actions;
using Tests.Voiseed.Base;

namespace Tests.Voiseed
{
    [TestClass]
    public class BatchActionTests : TestBase
    {
        [TestMethod]
        public async Task CreateBatch_IsSuccess()
        {
            var action = new BatchActions(InvocationContext, FileManager);
            var result = await action.CreateBatch(new()
            {
               ProjectId = "34d92d4b-a647-4ba6-bf59-6a3aac939db8",
               Name= "Test from API",
               AutomaticInference = true,
                NoOfAlternativeTakes = 0,
                LanguageId = "FR-FR",
                IncludeId = true,
                IncludeCharacter = true,
                IncludeEmotion = true,
                IncludeIntensity = true,
                Ids = new[] { 1, 2, 3 },
                Characters = new[] { "Narrator", "Hero", "Villain" },
                Emotions = new[] { "neutral", "happy", "angry" },
                Intensities = new[] { "normal", "strong", "strong" },
                Texts = new[]
                {
                    "Hello, this is a test.",
                    "We are creating a batch via API.",
                    "This is the third line."
                }
            });

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(json);
            Assert.IsNotNull(result);

        }

        [TestMethod]
        public async Task GetBatch_IsSuccess()
        {
            var action = new BatchActions(InvocationContext, FileManager);
            var result = await action.GetBatch("46dbf12c-3089-4a61-84d6-49744493f9af");

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(json);
            Assert.IsNotNull(result);

        }
    }
}
