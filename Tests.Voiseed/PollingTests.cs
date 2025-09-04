using Apps.Voiseed.Polling.Models;
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
            var memory = new DateMemory
            {
                //LastInteractionDate = DateTime.UtcNow.AddMinutes(-10),
                //LastStatus = "PENDING"
            };
            var request = new Blackbird.Applications.Sdk.Common.Polling.PollingEventRequest<DateMemory>
            {
                Memory = memory
            };
            string requestId = "4cc0c8b4-96fc-4bea-8450-87e966284db3";
            var response = await pollingList.OnConvertTextToSpeechCompleted(request, requestId);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            Console.WriteLine(json);

            Assert.IsNotNull(response);
            Assert.AreEqual("SUCCESS", response.Memory.LastStatus);
        }

        [TestMethod]
        public async Task OnBatchCompleted_ShouldReturnSuccess()
        {

            var pollingList = new Apps.Voiseed.Polling.PollingList(InvocationContext);
            var memory = new DateMemory
            {
                //LastInteractionDate = DateTime.UtcNow.AddMinutes(-10),
                //LastStatus = "PENDING"
            };
            var request = new Blackbird.Applications.Sdk.Common.Polling.PollingEventRequest<DateMemory>
            {
                Memory = memory
            };
            string batchId = "46dbf12c-3089-4a61-84d6-49744493f9af";
            var response = await pollingList.OnBatchCompleted(request, batchId);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            Console.WriteLine(json);

            Assert.IsNotNull(response);
            Assert.AreEqual("COMPLETED", response.Memory.LastStatus);
        }
    }
}
