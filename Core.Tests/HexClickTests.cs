using Bunit;
using DotDev.Client.Shared.HexPath;
using DotDev.Core.HexPath;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using Xunit;

namespace Core.Tests;

public class HexClickTests : TestContext
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

    // Loose mode + SetupModule: _mod is non-null, all module calls succeed
    private (IRenderedComponent<Hex> cut, Mock<IHoneycomb> mock, BunitJSInterop jsModule)
        RenderWithModule(HexItem item)
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        var jsModule = JSInterop.SetupModule("./js/consoleFeels.js");

        var mock = MakeMock();
        Services.AddSingleton(mock.Object);
        var cut = Render<Hex>(p => p.Add(c => c.Hx, item));
        return (cut, mock, jsModule);
    }

    private static StatusHex MakeEnabledStatusHex() =>
        new StatusHex(new HexLocation(HexOrder.Status, 3u, 5u), enable: true);

    private static StatusHex MakeGhostStatusHex() =>
        new StatusHex(new HexLocation(HexOrder.Status, 3u, 5u), enable: false);

    private static IntroHex MakeIntroHex() =>
        new IntroHex(new HexLocation(HexOrder.Intro, 2u, 5u));

    // --- Disabled hex ---

    [Fact]
    public void Click_WhenDisabled_DoesNotCallEnableGhosts()
    {
        var (cut, mock, _) = RenderWithModule(MakeGhostStatusHex());

        cut.Find("div").Click();

        mock.Verify(h => h.EnableGhosts(It.IsAny<HexLocation>()), Times.Never);
    }

    [Fact]
    public void Click_WhenDisabled_DoesNotNavigate()
    {
        var (cut, _, _) = RenderWithModule(MakeGhostStatusHex());
        var nav = Services.GetRequiredService<NavigationManager>();
        var uriBefore = nav.Uri;

        cut.Find("div").Click();

        Assert.Equal(uriBefore, nav.Uri);
    }

    [Fact]
    public void Click_WhenDisabled_DoesNotCallStopFeels()
    {
        var (cut, _, jsModule) = RenderWithModule(MakeGhostStatusHex());

        cut.Find("div").Click();

        jsModule.VerifyNotInvoke("stopFeels");
    }

    // --- Enabled hex with navigation target ---

    [Fact]
    public void Click_WhenEnabledWithTarget_Navigates()
    {
        var (cut, _, _) = RenderWithModule(MakeEnabledStatusHex());
        var nav = Services.GetRequiredService<NavigationManager>();

        cut.Find("div").Click();

        cut.WaitForAssertion(() => Assert.Contains("/Status", nav.Uri));
    }

    [Fact]
    public void Click_WhenEnabledWithTarget_CallsStopFeels()
    {
        var (cut, _, jsModule) = RenderWithModule(MakeEnabledStatusHex());

        cut.Find("div").Click();

        cut.WaitForAssertion(() => jsModule.VerifyInvoke("stopFeels"));
    }

    [Fact]
    public void Click_WhenEnabledWithTarget_CallsEnableGhostsViaHover()
    {
        var (cut, mock, _) = RenderWithModule(MakeEnabledStatusHex());

        cut.Find("div").Click();

        mock.Verify(h => h.EnableGhosts(It.IsAny<HexLocation>()), Times.Once);
    }

    [Fact]
    public void Click_WhenAlreadyOpened_DoesNotCallEnableGhostsAgain()
    {
        var (cut, mock, _) = RenderWithModule(MakeEnabledStatusHex());

        cut.Find("div").MouseOver(); // opens it
        cut.Find("div").Click();     // click after already opened

        mock.Verify(h => h.EnableGhosts(It.IsAny<HexLocation>()), Times.Once);
    }

    // --- Enabled hex without navigation target (#) ---

    [Fact]
    public void Click_WhenEnabledWithHashTarget_DoesNotNavigate()
    {
        var (cut, _, _) = RenderWithModule(MakeIntroHex());
        var nav = Services.GetRequiredService<NavigationManager>();
        var uriBefore = nav.Uri;

        cut.Find("div").Click();

        Assert.Equal(uriBefore, nav.Uri);
    }

    [Fact]
    public void Click_WhenEnabledWithHashTarget_CallsConsolePrint()
    {
        var (cut, _, jsModule) = RenderWithModule(MakeIntroHex());

        cut.Find("div").Click();

        cut.WaitForAssertion(() => jsModule.VerifyInvoke("consolePrint"));
    }

    // --- Hover ---

    [Fact]
    public void Hover_WhenEnabled_OpensHex()
    {
        var (cut, _, _) = RenderWithModule(MakeIntroHex());

        cut.Find("div").MouseOver();

        Assert.Contains("aqua", cut.Instance.PolygonStyle);
    }

    [Fact]
    public void Hover_WhenAlreadyOpened_PolygonStyleUnchanged()
    {
        var (cut, _, _) = RenderWithModule(MakeIntroHex());

        cut.Find("div").MouseOver();
        var styleAfterFirst = cut.Instance.PolygonStyle;
        cut.Find("div").MouseOver();

        Assert.Equal(styleAfterFirst, cut.Instance.PolygonStyle);
    }
}
