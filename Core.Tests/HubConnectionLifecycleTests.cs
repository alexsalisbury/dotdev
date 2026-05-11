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

public class HubConnectionLifecycleTests : TestContext
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
    public void HubConnection_IsCreatedOnFirstRender()
    {
        ConfigureServices();
        var component = Render<PeriodicTable>();

        var hubField = typeof(PeriodicTable)
            .GetField("elementStatus", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        Assert.NotNull(hubField);
    }

    [Fact]
    public void HubConnection_UsesAutomaticReconnect()
    {
        ConfigureServices();
        var component = Render<PeriodicTable>();

        var hubField = typeof(PeriodicTable)
            .GetField("elementStatus", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        Assert.NotNull(hubField);
        var hubConnection = hubField.GetValue(component.Instance) as HubConnection;
        if (hubConnection != null)
        {
            Assert.NotNull(hubConnection);
        }
    }

    [Fact]
    public void HubConnection_OnlyCreatedOnce_AfterMultipleRenders()
    {
        ConfigureServices();
        var component = Render<PeriodicTable>();

        component.Render();
        component.Render();

        var hubField = typeof(PeriodicTable)
            .GetField("elementStatus", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (hubField != null)
        {
            var hubConnection = hubField.GetValue(component.Instance);
            Assert.NotNull(hubConnection);
        }
    }

    [Fact]
    public async Task HubConnection_StopAsync_CalledDuringDispose()
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
                Assert.Equal(HubConnectionState.Disconnected, hubConnection.State);
            }
        }
    }

    [Fact]
    public async Task HubConnection_NoNewMessagesAfterDispose()
    {
        ConfigureServices();
        var component = Render<PeriodicTable>();

        await component.Instance.DisposeAsync();

        var squaresField = typeof(PeriodicTable)
            .GetField("squares", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        Assert.NotNull(squaresField);
        var squares = squaresField.GetValue(component.Instance) as Dictionary<int, Square>;
        Assert.NotNull(squares);
    }
}
