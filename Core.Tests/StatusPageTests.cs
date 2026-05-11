using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Bunit;
using DotDev.Client.Pages;
using DotDev.Client.Shared.Status;
using DotDev.Core.Element;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Core.Tests;

public class StatusPageTests : TestContext
{
    private void ConfigureServices()
    {
        var mockHandler = new MockHttpMessageHandler();
        var httpClient = new HttpClient(mockHandler)
        {
            BaseAddress = new Uri("http://localhost/")
        };
        Services.AddSingleton(httpClient);
        Services.AddSingleton<ILogger<PeriodicTable>>(new Mock<ILogger<PeriodicTable>>().Object);
    }

    [Fact]
    public void Status_RendersWithoutCrash()
    {
        ConfigureServices();
        var cut = Render<Status>();
        Assert.NotNull(cut);
    }

    [Fact]
    public void Status_RendersPeriodicTable()
    {
        ConfigureServices();
        var cut = Render<Status>();
        var periodicTable = cut.Find(".periodic-table");
        Assert.NotNull(periodicTable);
    }

    [Fact]
    public async Task Status_DisposeAsync_CanBeCalledSafely()
    {
        ConfigureServices();
        var cut = Render<Status>();
        var periodicTable = cut.FindComponent<PeriodicTable>();

        var exception = await Record.ExceptionAsync(async () =>
        {
            await periodicTable.Instance.DisposeAsync();
        });

        Assert.Null(exception);
    }
}
