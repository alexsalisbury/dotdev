using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Bunit;
using DotDev.Client.Shared.Status;
using DotDev.Core.Element;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Core.Tests;

public class StatusPageNavigationTests : TestContext
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
    public async Task ComponentDisposal_CleansUpHubConnection()
    {
        ConfigureServices();
        var component = Render<PeriodicTable>();

        await component.Instance.DisposeAsync();

        var hubField = typeof(PeriodicTable)
            .GetField("elementStatus", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        Assert.NotNull(hubField);

        var hubConnection = hubField.GetValue(component.Instance) as HubConnection;
        Assert.True(
            hubConnection == null || hubConnection.State == HubConnectionState.Disconnected,
            "Connection should be cleaned up when user navigates away from dashboard"
        );
    }

    [Fact]
    public async Task ComponentDisposal_PreventsConnectionLeak()
    {
        ConfigureServices();
        var components = new List<IRenderedComponent<PeriodicTable>>();

        for (int i = 0; i < 3; i++)
        {
            var component = Render<PeriodicTable>();
            components.Add(component);
        }

        foreach (var component in components)
        {
            await component.Instance.DisposeAsync();
        }

        foreach (var component in components)
        {
            var hubField = typeof(PeriodicTable)
                .GetField("elementStatus", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (hubField != null)
            {
                var hubConnection = hubField.GetValue(component.Instance) as HubConnection;
                Assert.True(
                    hubConnection == null || hubConnection.State == HubConnectionState.Disconnected,
                    "All connections should be cleaned up after multiple navigations to/from dashboard"
                );
            }
        }
    }

    [Fact]
    public async Task ComponentDisposal_HandlesNullHubConnection()
    {
        ConfigureServices();
        var component = Render<PeriodicTable>();

        var hubField = typeof(PeriodicTable)
            .GetField("elementStatus", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (hubField != null)
        {
            hubField.SetValue(component.Instance, null);
        }

        var exception = await Record.ExceptionAsync(async () =>
        {
            await component.Instance.DisposeAsync();
        });

        Assert.Null(exception);
    }

    [Fact]
    public async Task MultipleDisposeCalls_DoNotThrow()
    {
        ConfigureServices();
        var component = Render<PeriodicTable>();

        await component.Instance.DisposeAsync();

        var exception = await Record.ExceptionAsync(async () =>
        {
            await component.Instance.DisposeAsync();
        });

        Assert.Null(exception);
    }

    [Fact]
    public async Task DisposeAsync_StopsConnectionBeforeDisposing()
    {
        ConfigureServices();
        var component = Render<PeriodicTable>();

        await component.Instance.DisposeAsync();

        var hubField = typeof(PeriodicTable)
            .GetField("elementStatus", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (hubField != null)
        {
            var hubConnection = hubField.GetValue(component.Instance) as HubConnection;
            if (hubConnection != null)
            {
                Assert.NotEqual(HubConnectionState.Connected, hubConnection.State);
                Assert.NotEqual(HubConnectionState.Connecting, hubConnection.State);
                Assert.NotEqual(HubConnectionState.Reconnecting, hubConnection.State);
            }
        }
    }
}
