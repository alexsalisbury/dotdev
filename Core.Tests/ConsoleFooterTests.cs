using Bunit;
using DotDev.Client.Shared;
using Microsoft.JSInterop;
using Xunit;

namespace Core.Tests;

public class ConsoleFooterTests : TestContext
{
    [Fact]
    public void ConsoleFooter_RendersWithoutCrash()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var cut = Render<ConsoleFooter>(p => p.Add(c => c.Lines, new[] { "line1", "line2" }));
        Assert.NotNull(cut);
    }

    [Fact]
    public void ConsoleFooter_ContainsConsoleTargetDiv()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var cut = Render<ConsoleFooter>(p => p.Add(c => c.Lines, new[] { "line1" }));
        var div = cut.Find("#consoleTarget");
        Assert.NotNull(div);
    }

    [Fact]
    public void ConsoleFooter_AcceptsNullLines()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var cut = Render<ConsoleFooter>(p => p.Add(c => c.Lines, (string[])null!));
        Assert.NotNull(cut);
    }

    [Fact]
    public void ConsoleFooter_AcceptsEmptyLines()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var cut = Render<ConsoleFooter>(p => p.Add(c => c.Lines, Array.Empty<string>()));
        Assert.NotNull(cut);
    }

    [Fact]
    public async Task ConsoleFooter_DisposeAsync_SafeWhenModIsNull()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var cut = Render<ConsoleFooter>(p => p.Add(c => c.Lines, new[] { "hello" }));

        var exception = await Record.ExceptionAsync(async () =>
        {
            await cut.Instance.DisposeAsync();
        });

        Assert.Null(exception);
    }

    [Fact]
    public async Task ConsoleFooter_DisposeAsync_CanBeCalledMultipleTimes()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var cut = Render<ConsoleFooter>(p => p.Add(c => c.Lines, new[] { "hello" }));

        var exception = await Record.ExceptionAsync(async () =>
        {
            await cut.Instance.DisposeAsync();
            await cut.Instance.DisposeAsync();
        });

        Assert.Null(exception);
    }

    [Fact]
    public void ConsoleFooter_ImplementsIAsyncDisposable()
    {
        Assert.True(typeof(IAsyncDisposable).IsAssignableFrom(typeof(ConsoleFooter)));
    }

    [Fact]
    public void ConsoleFooter_LinesParameter_IsAccepted()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var lines = new[] { "Welcome", "1/3 modules loaded" };
        var cut = Render<ConsoleFooter>(p => p.Add(c => c.Lines, lines));
        Assert.Equal(lines, cut.Instance.Lines);
    }
}
