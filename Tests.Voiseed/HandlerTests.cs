using Apps.Voiseed.Handlers;
using Apps.Voiseed.DataHandlers;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Tests.Voiseed.Base;

namespace Tests.Voiseed;

[TestClass]
public class HandlerTests : TestBase
{
    [TestMethod]
    public async Task VoicesDataHandler_IsSuccess()
    {
        var handler = new VoicesDataHandler(InvocationContext);
        var response = await handler.GetDataAsync(new DataSourceContext(), CancellationToken.None);

        foreach (var item in response)
        {
            Console.WriteLine($"{item.DisplayName} - {item.Value}");
        }

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public async Task ModelsDataHandler_IsSuccess()
    {
        var handler = new ModelsDataHandler(InvocationContext);
        var response = await handler.GetDataAsync(new DataSourceContext(), CancellationToken.None);

        foreach (var item in response)
        {
            Console.WriteLine($"{item.DisplayName} - {item.Value}");
        }

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public async Task LanguagesDataHandler_IsSuccess()
    {
        var handler = new LanguagesDataHandler(InvocationContext);
        var response = await handler.GetDataAsync(new DataSourceContext(), CancellationToken.None);

        foreach (var item in response)
        {
            Console.WriteLine($"{item.DisplayName} - {item.Value}");
        }

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public async Task GlossariesDataHandler_IsSuccess()
    {
        var handler = new GlossariesDataHandler(InvocationContext);
        var response = await handler.GetDataAsync(new DataSourceContext(), CancellationToken.None);

        foreach (var item in response)
        {
            Console.WriteLine($"{item.DisplayName} - {item.Value}");
        }

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public async Task VoiceStylesDataHandler_IsSuccess()
    {
        var handler = new VoiceStylesDataHandler(InvocationContext, "xpressive");
        var response = await handler.GetDataAsync(new DataSourceContext(), CancellationToken.None);

        foreach (var item in response)
        {
            Console.WriteLine($"{item.DisplayName} - {item.Value}");
        }

        Assert.IsNotNull(response);
    }
}
