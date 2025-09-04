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
            var result = await action.GetBatch("1cb13d69-f4b7-495a-89fa-dcf0035b4b69");

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(json);
            Assert.IsNotNull(result);

        }
    }
}
