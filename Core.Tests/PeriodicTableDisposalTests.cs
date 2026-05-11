using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Bunit;
using DotDev.Client.Shared.Status;
using DotDev.Core.Element;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Core.Tests;

public class PeriodicTableDisposalTests : TestContext
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
    public void PeriodicTable_ImplementsIAsyncDisposable()
    {
        Assert.True(typeof(IAsyncDisposable).IsAssignableFrom(typeof(PeriodicTable)));
    }

    [Fact]
    public async Task PeriodicTable_DisposeAsync_CanBeCalledSafely()
    {
        ConfigureServices();
        var component = Render<PeriodicTable>();

        var exception = await Record.ExceptionAsync(async () =>
        {
            await component.Instance.DisposeAsync();
        });

        Assert.Null(exception);
    }

    [Fact]
    public async Task PeriodicTable_DisposeAsync_CanBeCalledMultipleTimes()
    {
        ConfigureServices();
        var component = Render<PeriodicTable>();

        var exception = await Record.ExceptionAsync(async () =>
        {
            await component.Instance.DisposeAsync();
            await component.Instance.DisposeAsync();
        });

        Assert.Null(exception);
    }

    [Fact]
    public void PeriodicTable_DisposeAsync_StopsHubConnection()
    {
        ConfigureServices();
        var component = Render<PeriodicTable>();

        component.Instance.DisposeAsync().AsTask().Wait();

        var hubField = typeof(PeriodicTable)
            .GetField("elementStatus", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (hubField != null)
        {
            var hubConnection = hubField.GetValue(component.Instance) as HubConnection;
            if (hubConnection != null)
            {
                Assert.True(
                    hubConnection.State == HubConnectionState.Disconnected,
                    "HubConnection should be disconnected after disposal"
                );
            }
        }
    }

    [Fact]
    public void PeriodicTable_RendersWithoutCrash()
    {
        ConfigureServices();
        var component = Render<PeriodicTable>();
        Assert.NotNull(component);
    }

    [Fact]
    public void PeriodicTable_ContainsPeriodicTableDiv()
    {
        ConfigureServices();
        var component = Render<PeriodicTable>();
        var div = component.Find(".periodic-table");
        Assert.NotNull(div);
    }

    [Fact]
    public async Task PeriodicTable_NavigatingAway_DisposesConnection()
    {
        ConfigureServices();
        var component = Render<PeriodicTable>();

        await component.Instance.DisposeAsync();

        var hubField = typeof(PeriodicTable)
            .GetField("elementStatus", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (hubField != null)
        {
            var hubConnection = hubField.GetValue(component.Instance) as HubConnection;
            Assert.True(
                hubConnection == null || hubConnection.State == HubConnectionState.Disconnected,
                "HubConnection must be stopped/disposed when navigating away from the page"
            );
        }
    }

    [Fact]
    public async Task PeriodicTable_DisposeAsync_SetsHubConnectionToNull()
    {
        ConfigureServices();
        var component = Render<PeriodicTable>();
        await Task.Delay(1500); // Wait for the connection to establish);
        await component.Instance.DisposeAsync();

        var hubField = typeof(PeriodicTable)
            .GetField("elementStatus", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        Assert.NotNull(hubField);
        var hubConnection = hubField.GetValue(component.Instance);
        Assert.Null(hubConnection);
    }
}

public class MockHttpMessageHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var uri = request.RequestUri?.PathAndQuery ?? "";

        if (uri.Contains("ElementFetch"))
        {
            var elements = new[]
            {
                new ElementInfo(1, "H", "Hydrogen", 1.008, "[1]", "other-nonmetal", 1, 1),
                new ElementInfo(2, "He", "Helium", 4.0026, "[2]", "noble-gas", 18, 1)
            };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(elements)
            };
            return Task.FromResult(response);
        }

        if (uri.Contains("ServerFetch"))
        {
            var servers = Array.Empty<ServerInfo>();
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(servers)
            };
            return Task.FromResult(response);
        }

        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
    }
}
