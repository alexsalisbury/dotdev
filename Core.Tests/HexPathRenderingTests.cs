using Bunit;
using DotDev.Client.Shared.HexPath;
using DotDev.Core.HexPath;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using Xunit;

namespace Core.Tests;

public class HexPathRenderingTests : TestContext
{
    private Mock<IHoneycomb> MakeMock(params HexItem[] items)
    {
        var mock = new Mock<IHoneycomb>();
        mock.Setup(h => h.GetItems()).Returns(items);
        mock.Setup(h => h.AddRoot(It.IsAny<HexLocation>())).Returns(true);
        mock.Setup(h => h.AddGhosts(It.IsAny<HexLocation>()));
        mock.Setup(h => h.EnableGhosts(It.IsAny<HexLocation>()));
        mock.Setup(h => h.UnlockAsync()).Returns(Task.CompletedTask);
        return mock;
    }

    private IRenderedComponent<HexPath> RenderHexPath(Mock<IHoneycomb> mock)
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(mock.Object);
        return Render<HexPath>(p => p.Add(c => c.Grid, mock.Object));
    }

    private static IntroHex MakeIntroHex() =>
        new IntroHex(new HexLocation(HexOrder.Intro, 2u, 5u));

    private static StatusHex MakeStatusHex() =>
        new StatusHex(new HexLocation(HexOrder.Status, 3u, 5u), enable: true);

    [Fact]
    public void HexPath_WithNoItems_RendersEmptyHexPathDiv()
    {
        var mock = MakeMock();
        var cut = RenderHexPath(mock);
        Assert.Empty(cut.FindAll(".hexPath > *"));
    }

    [Fact]
    public void HexPath_WithOneItem_RendersOneChild()
    {
        var mock = MakeMock(MakeIntroHex());
        var cut = RenderHexPath(mock);
        Assert.Single(cut.FindAll(".hexPath > div"));
    }

    [Fact]
    public void HexPath_WithTwoItems_RendersTwoChildren()
    {
        var mock = MakeMock(MakeIntroHex(), MakeStatusHex());
        var cut = RenderHexPath(mock);
        Assert.Equal(2, cut.FindAll(".hexPath > div").Count);
    }

    [Fact]
    public void HexPath_WithItem_RendersHexClassFromStyle()
    {
        var mock = MakeMock(MakeIntroHex());
        var cut = RenderHexPath(mock);
        var child = cut.Find(".hexPath > div");
        Assert.Contains("hexroot", child.ClassName ?? string.Empty);
    }

    [Fact]
    public void HexPath_WithGhostItem_RendersGhostClass()
    {
        // BlankItem with null HexClass exercises the "hexghost" fallback in Hex.HexClass
        var location = new HexLocation(HexOrder.Blank, 1u, 1u);
        var ghostStyle = new HexStyle { HexClass = null, IsGhost = true };
        var ghost = new BlankItem(location, ghostStyle);
        var mock = MakeMock(ghost);
        var cut = RenderHexPath(mock);
        var child = cut.Find(".hexPath > div");
        Assert.Contains("hexghost", child.ClassName ?? string.Empty);
    }

    [Fact]
    public async Task HexPath_HoneycombChangedEvent_TriggersReRender()
    {
        var mock = MakeMock();
        var cut = RenderHexPath(mock);

        // Event must be raised on the Blazor dispatcher
        mock.Setup(h => h.GetItems()).Returns(new[] { MakeIntroHex() });
        await cut.InvokeAsync(() =>
            mock.Raise(h => h.HoneycombChanged += null, EventArgs.Empty));

        Assert.Single(cut.FindAll(".hexPath > div"));
    }

    [Fact]
    public void HexPath_WithItem_SvgIsRendered()
    {
        var mock = MakeMock(MakeIntroHex());
        var cut = RenderHexPath(mock);
        var svg = cut.Find("svg");
        Assert.NotNull(svg);
    }

    [Fact]
    public void HexPath_AddRootCalledWithIntroOrder()
    {
        var mock = MakeMock();
        RenderHexPath(mock);
        mock.Verify(
            h => h.AddRoot(It.Is<HexLocation>(l => l.Index == HexOrder.Intro)),
            Times.Once);
    }
}
