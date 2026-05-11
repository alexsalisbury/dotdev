using Bunit;
using DotDev.Core.HexPath;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using Xunit;
using IndexPage = DotDev.Client.Pages.Index;

namespace Core.Tests;

public class IndexPageTests : TestContext
{
    private Mock<IHoneycomb> MakeMock()
    {
        var mock = new Mock<IHoneycomb>();
        mock.Setup(h => h.GetItems()).Returns(Array.Empty<HexItem>());
        mock.Setup(h => h.AddRoot(It.IsAny<HexLocation>())).Returns(true);
        mock.Setup(h => h.AddGhosts(It.IsAny<HexLocation>()));
        mock.Setup(h => h.EnableGhosts(It.IsAny<HexLocation>()));
        mock.Setup(h => h.UnlockAsync()).Returns(Task.CompletedTask);
        return mock;
    }

    private IRenderedComponent<IndexPage> RenderIndex()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var mock = MakeMock();
        Services.AddSingleton(mock.Object);
        return Render<IndexPage>();
    }

    [Fact]
    public void Index_RendersWithoutCrash()
    {
        var cut = RenderIndex();
        Assert.NotNull(cut);
    }

    [Fact]
    public void Index_RendersHexPathComponent()
    {
        var cut = RenderIndex();
        var hexPath = cut.Find(".hexPath");
        Assert.NotNull(hexPath);
    }

    [Fact]
    public void Index_RendersConsoleFooterComponent()
    {
        var cut = RenderIndex();
        var consoleTarget = cut.Find("#consoleTarget");
        Assert.NotNull(consoleTarget);
    }

    [Fact]
    public void Index_Lines_DefaultsToIntroHexDefaultText()
    {
        var cut = RenderIndex();
        Assert.Equal(IntroHex.DefaultText, cut.Instance.Lines);
    }

    [Fact]
    public void Index_CallsAddRootOnInitialized()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var mock = MakeMock();
        Services.AddSingleton(mock.Object);
        Render<IndexPage>();
        mock.Verify(h => h.AddRoot(It.IsAny<HexLocation>()), Times.Once);
    }
}
