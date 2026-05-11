using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using Bunit;
using DotDev.Client.Shared.Status;
using DotDev.Core.Element;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Core.Tests;

public class PeriodicTableRenderingTests : TestContext
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
    public void PeriodicTable_DefaultElements_RendersSquares()
    {
        ConfigureServices();
        var cut = Render<PeriodicTable>();
        var squares = cut.FindAll(".square");
        Assert.NotEmpty(squares);
    }

    [Fact]
    public void PeriodicTable_DefaultElements_Renders118Squares()
    {
        ConfigureServices();
        var cut = Render<PeriodicTable>();
        var squares = cut.FindAll(".square");
        Assert.Equal(118, squares.Count);
    }

    [Fact]
    public void PeriodicTable_DefaultElements_RendersHydrogen()
    {
        ConfigureServices();
        var cut = Render<PeriodicTable>();
        // Hydrogen is at column 1, row 1 → class "square c1 r1"
        var h = cut.Find(".square.c1.r1");
        Assert.NotNull(h);
    }

    [Fact]
    public void PeriodicTable_DefaultElements_RendersHelium()
    {
        ConfigureServices();
        var cut = Render<PeriodicTable>();
        // Helium is at column 18, row 1
        var he = cut.Find(".square.c18.r1");
        Assert.NotNull(he);
    }

    [Fact]
    public void PeriodicTable_DefaultElements_AllSquaresShowUnknownStatusInitially()
    {
        ConfigureServices();
        var cut = Render<PeriodicTable>();
        // Element.razor renders status in a .status div; all default squares have no server data
        var statusDivs = cut.FindAll(".status");
        Assert.All(statusDivs, div => Assert.Equal("unknown", div.TextContent));
    }

    [Fact]
    public void PeriodicTable_SquaresField_IsPopulatedAfterInit()
    {
        ConfigureServices();
        var cut = Render<PeriodicTable>();
        var squaresField = typeof(PeriodicTable)
            .GetField("squares", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.NotNull(squaresField);
        var squares = squaresField.GetValue(cut.Instance) as Dictionary<int, Square>;
        Assert.NotNull(squares);
        Assert.Equal(118, squares.Count);
    }

    [Fact]
    public void PeriodicTable_SquaresField_ContainsHydrogen()
    {
        ConfigureServices();
        var cut = Render<PeriodicTable>();
        var squaresField = typeof(PeriodicTable)
            .GetField("squares", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var squares = squaresField!.GetValue(cut.Instance) as Dictionary<int, Square>;
        Assert.True(squares!.ContainsKey(1));
        Assert.Equal("Hydrogen", squares[1].Info?.ElementName);
    }

    [Fact]
    public void PeriodicTable_SquaresField_AllSquaresHaveElementInfo()
    {
        ConfigureServices();
        var cut = Render<PeriodicTable>();
        var squaresField = typeof(PeriodicTable)
            .GetField("squares", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var squares = squaresField!.GetValue(cut.Instance) as Dictionary<int, Square>;
        Assert.All(squares!.Values, sq => Assert.NotNull(sq.Info));
    }

    [Fact]
    public void PeriodicTable_SquaresField_NoServerStatusInitially()
    {
        ConfigureServices();
        var cut = Render<PeriodicTable>();
        var squaresField = typeof(PeriodicTable)
            .GetField("squares", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var squares = squaresField!.GetValue(cut.Instance) as Dictionary<int, Square>;
        Assert.All(squares!.Values, sq => Assert.Null(sq.Status));
    }

    [Fact]
    public void PeriodicTable_SquaresField_AllSquaresServerStatusClassIsUnknown()
    {
        ConfigureServices();
        var cut = Render<PeriodicTable>();
        var squaresField = typeof(PeriodicTable)
            .GetField("squares", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var squares = squaresField!.GetValue(cut.Instance) as Dictionary<int, Square>;
        Assert.All(squares!.Values, sq => Assert.Equal("unknown", sq.ServerStatusClass));
    }
}
