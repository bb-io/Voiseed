using Tests.Voiseed.Base;

namespace Tests.Voiseed
{
    [TestClass]
    public class PollingTests : TestBase
    {

        [TestMethod]
        public async Task OnConvertTextToSpeechCompleted_ShouldReturnSuccess()
        {

            var pollingList = new Apps.Voiseed.Polling.PollingList(InvocationContext);
            var memory = new Apps.Voiseed.Polling.Models.DateMemory
            {
                LastInteractionDate = DateTime.UtcNow.AddMinutes(-10),
                LastStatus = "PENDING"
            };
            var request = new Blackbird.Applications.Sdk.Common.Polling.PollingEventRequest<Apps.Voiseed.Polling.Models.DateMemory>
            {
                Memory = memory
            };
            string requestId = "9e08a0b4-2328-4e16-9012-4280ffba783b";
            var response = await pollingList.OnConvertTextToSpeechCompleted(request, requestId);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            Console.WriteLine(json);

            Assert.IsNotNull(response);
            Assert.AreEqual("SUCCESS", response.Memory.LastStatus);
        }
    }
}
