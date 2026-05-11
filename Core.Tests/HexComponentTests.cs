using Bunit;
using DotDev.Client.Shared.HexPath;
using DotDev.Core.HexPath;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using Xunit;

namespace Core.Tests;

public class HexComponentTests : TestContext
{
    private Mock<IHoneycomb> MakeMock()
    {
        var mock = new Mock<IHoneycomb>();
        mock.Setup(h => h.GetItems()).Returns(Array.Empty<HexItem>());
        mock.Setup(h => h.AddRoot(It.IsAny<HexLocation>())).Returns(true);
        mock.Setup(h => h.AddGhosts(It.IsAny<HexLocation>()));
        mock.Setup(h => h.EnableGhosts(It.IsAny<HexLocation>()));
        return mock;
    }

    private static IntroHex MakeIntroHex(bool isGhost = false)
    {
        var location = new HexLocation(HexOrder.Intro, 2, 5);
        var hex = new IntroHex(location);
        if (isGhost)
        {
            var ghostStyle = IntroHex.DefaultStyle with { IsGhost = true, HexClass = "hexghost" };
            return hex with { Style = ghostStyle };
        }
        return hex;
    }

    private IRenderedComponent<Hex> RenderHex(HexItem item)
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var mock = MakeMock();
        Services.AddSingleton(mock.Object);
        return Render<Hex>(p => p.Add(c => c.Hx, item));
    }

    [Fact]
    public void Hex_RendersWithoutCrash()
    {
        var cut = RenderHex(MakeIntroHex());
        Assert.NotNull(cut);
    }

    [Fact]
    public void Hex_ContainsSvgElement()
    {
        var cut = RenderHex(MakeIntroHex());
        var svg = cut.Find("svg");
        Assert.NotNull(svg);
    }

    [Fact]
    public void Hex_ContainsPolygon()
    {
        var cut = RenderHex(MakeIntroHex());
        var polygon = cut.Find("polygon");
        Assert.NotNull(polygon);
    }

    [Fact]
    public void Hex_Index_ReturnsLocationIndex()
    {
        var cut = RenderHex(MakeIntroHex());
        // HexOrder is an enum; string interpolation yields the enum name, not the numeric value
        Assert.Equal($"{HexOrder.Intro}", cut.Instance.Index);
    }

    [Fact]
    public void Hex_ImgPattern_IncludesIndex()
    {
        var cut = RenderHex(MakeIntroHex());
        Assert.Contains(cut.Instance.Index, cut.Instance.ImgPattern);
        Assert.StartsWith("imgpattern", cut.Instance.ImgPattern);
    }

    [Fact]
    public void Hex_ImgPatternUrl_WrapsImgPattern()
    {
        var cut = RenderHex(MakeIntroHex());
        Assert.Equal($"url(#{cut.Instance.ImgPattern})", cut.Instance.ImgPatternUrl);
    }

    [Fact]
    public void Hex_HexClass_UsesStyleHexClass()
    {
        var cut = RenderHex(MakeIntroHex());
        Assert.Equal("hexroot", cut.Instance.HexClass);
    }

    [Fact]
    public void Hex_HexClass_DefaultsToHexghostWhenStyleIsNull()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var mock = MakeMock();
        Services.AddSingleton(mock.Object);
        var location = new HexLocation(HexOrder.Blank, 1, 1);
        var style = new HexStyle { HexClass = null, IsGhost = true };
        var item = new BlankItem(location, style);
        var cut = Render<Hex>(p => p.Add(c => c.Hx, item));
        Assert.Equal("hexghost", cut.Instance.HexClass);
    }

    [Fact]
    public void Hex_Enabled_TrueWhenNotGhost()
    {
        var cut = RenderHex(MakeIntroHex(isGhost: false));
        Assert.True(cut.Instance.Enabled);
    }

    [Fact]
    public void Hex_Enabled_FalseWhenGhost()
    {
        var cut = RenderHex(MakeIntroHex(isGhost: true));
        Assert.False(cut.Instance.Enabled);
    }

    [Fact]
    public void Hex_PolygonStyle_StartsAsDashArray()
    {
        var cut = RenderHex(MakeIntroHex());
        Assert.Contains("stroke-dasharray", cut.Instance.PolygonStyle);
    }

    [Fact]
    public void Hex_HexFill_WhenGhost_IsTransparent()
    {
        var cut = RenderHex(MakeIntroHex(isGhost: true));
        Assert.Equal("transparent", cut.Instance.HexFill);
    }

    [Fact]
    public void Hex_HexFill_WhenEnabled_UsesShade()
    {
        var cut = RenderHex(MakeIntroHex());
        Assert.Equal(IntroHex.DefaultStyle.Shade, cut.Instance.HexFill);
    }

    [Fact]
    public void Hex_SvgClass_IncludesLocationIndex()
    {
        var cut = RenderHex(MakeIntroHex());
        Assert.Contains(((uint)HexOrder.Intro).ToString(), cut.Instance.SvgClass);
    }

    [Fact]
    public void Hex_Target_DefaultsToHashWhenNullInStyle()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var mock = MakeMock();
        Services.AddSingleton(mock.Object);
        var location = new HexLocation(HexOrder.Blank, 1, 1);
        var style = new HexStyle { Target = null, IsGhost = false };
        var item = new BlankItem(location, style);
        var cut = Render<Hex>(p => p.Add(c => c.Hx, item));
        Assert.Equal("#", cut.Instance.Target);
    }

    [Fact]
    public void Hex_OnInitialized_CallsAddGhosts()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var mock = MakeMock();
        Services.AddSingleton(mock.Object);
        var hex = MakeIntroHex();
        Render<Hex>(p => p.Add(c => c.Hx, hex));
        mock.Verify(h => h.AddGhosts(It.IsAny<HexLocation>()), Times.Once);
    }

    [Fact]
    public void Hex_OnHover_WhenEnabled_CallsEnableGhosts()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var mock = MakeMock();
        Services.AddSingleton(mock.Object);
        var hex = MakeIntroHex();
        var cut = Render<Hex>(p => p.Add(c => c.Hx, hex));

        cut.Find("div").MouseOver();

        mock.Verify(h => h.EnableGhosts(It.IsAny<HexLocation>()), Times.Once);
    }

    [Fact]
    public void Hex_OnHover_WhenEnabled_SetsOpenedTrue()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var mock = MakeMock();
        Services.AddSingleton(mock.Object);
        var hex = MakeIntroHex();
        var cut = Render<Hex>(p => p.Add(c => c.Hx, hex));

        cut.Find("div").MouseOver();

        Assert.Contains("aqua", cut.Instance.PolygonStyle);
    }

    [Fact]
    public void Hex_OnHover_WhenAlreadyOpened_DoesNotCallEnableGhostsAgain()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var mock = MakeMock();
        Services.AddSingleton(mock.Object);
        var hex = MakeIntroHex();
        var cut = Render<Hex>(p => p.Add(c => c.Hx, hex));

        cut.Find("div").MouseOver();
        cut.Find("div").MouseOver();

        mock.Verify(h => h.EnableGhosts(It.IsAny<HexLocation>()), Times.Once);
    }

    [Fact]
    public void Hex_OnHover_WhenGhost_DoesNotCallEnableGhosts()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var mock = MakeMock();
        Services.AddSingleton(mock.Object);
        var hex = MakeIntroHex(isGhost: true);
        var cut = Render<Hex>(p => p.Add(c => c.Hx, hex));

        cut.Find("div").MouseOver();

        mock.Verify(h => h.EnableGhosts(It.IsAny<HexLocation>()), Times.Never);
    }

    [Fact]
    public async Task Hex_DisposeAsync_CanBeCalledSafely()
    {
        var cut = RenderHex(MakeIntroHex());

        var exception = await Record.ExceptionAsync(async () =>
        {
            await cut.Instance.DisposeAsync();
        });

        Assert.Null(exception);
    }

    [Fact]
    public void Hex_ImplementsIAsyncDisposable()
    {
        Assert.True(typeof(IAsyncDisposable).IsAssignableFrom(typeof(Hex)));
    }

    [Fact]
    public void Hex_Image_DefaultsToHashWhenStyleImageIsNull()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var mock = MakeMock();
        Services.AddSingleton(mock.Object);
        var location = new HexLocation(HexOrder.Blank, 1, 1);
        var style = new HexStyle { Image = null, IsGhost = false };
        var item = new BlankItem(location, style);
        var cut = Render<Hex>(p => p.Add(c => c.Hx, item));
        Assert.Equal("#", cut.Instance.Image);
    }

    [Fact]
    public void Hex_Image_ReturnsStyleImage()
    {
        var cut = RenderHex(MakeIntroHex());
        Assert.Equal(IntroHex.DefaultStyle.Image, cut.Instance.Image);
    }
}
