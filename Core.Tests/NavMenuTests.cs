using Bunit;
using DotDev.Client.Layout;
using Xunit;

namespace Core.Tests;

public class LayoutNavMenuTests : TestContext
{
    [Fact]
    public void LayoutNavMenu_RendersWithoutCrash()
    {
        var cut = Render<NavMenu>();
        Assert.NotNull(cut);
    }

    [Fact]
    public void LayoutNavMenu_ContainsNavbarBrand()
    {
        var cut = Render<NavMenu>();
        var brand = cut.Find(".navbar-brand");
        Assert.NotNull(brand);
    }

    [Fact]
    public void LayoutNavMenu_ContainsToggleButton()
    {
        var cut = Render<NavMenu>();
        var button = cut.Find(".navbar-toggler");
        Assert.NotNull(button);
    }

    [Fact]
    public void LayoutNavMenu_NavIsCollapsedByDefault()
    {
        var cut = Render<NavMenu>();
        var nav = cut.Find(".nav-scrollable");
        Assert.Contains("collapse", nav.ClassName ?? string.Empty);
    }

    [Fact]
    public void LayoutNavMenu_ClickingToggle_ExpandsNav()
    {
        var cut = Render<NavMenu>();
        cut.Find(".navbar-toggler").Click();
        var nav = cut.Find(".nav-scrollable");
        Assert.DoesNotContain("collapse", nav.ClassName ?? string.Empty);
    }

    [Fact]
    public void LayoutNavMenu_ClickingToggleTwice_CollapsesNav()
    {
        var cut = Render<NavMenu>();
        cut.Find(".navbar-toggler").Click();
        cut.Find(".nav-scrollable").Click();
        var nav = cut.Find(".nav-scrollable");
        Assert.Contains("collapse", nav.ClassName ?? string.Empty);
    }

    [Fact]
    public void LayoutNavMenu_ContainsHomeNavLink()
    {
        var cut = Render<NavMenu>();
        var links = cut.FindAll("a.nav-link");
        Assert.Contains(links, a => a.TextContent.Contains("Home"));
    }

    [Fact]
    public void LayoutNavMenu_ContainsNavigation()
    {
        var cut = Render<NavMenu>();
        var nav = cut.Find("nav");
        Assert.NotNull(nav);
    }
}

public class SharedNavMenuTests : TestContext
{
    [Fact]
    public void SharedNavMenu_RendersWithoutCrash()
    {
        var cut = Render<DotDev.Client.Shared.NavMenu>();
        Assert.NotNull(cut);
    }

    [Fact]
    public void SharedNavMenu_ContainsNavbarBrand()
    {
        var cut = Render<DotDev.Client.Shared.NavMenu>();
        var brand = cut.Find(".navbar-brand");
        Assert.NotNull(brand);
    }

    [Fact]
    public void SharedNavMenu_ContainsToggleButton()
    {
        var cut = Render<DotDev.Client.Shared.NavMenu>();
        var button = cut.Find(".navbar-toggler");
        Assert.NotNull(button);
    }

    [Fact]
    public void SharedNavMenu_NavIsCollapsedByDefault()
    {
        var cut = Render<DotDev.Client.Shared.NavMenu>();
        var collapsedDiv = cut.Find("div.collapse");
        Assert.NotNull(collapsedDiv);
    }

    [Fact]
    public void SharedNavMenu_ClickingToggle_ExpandsNav()
    {
        var cut = Render<DotDev.Client.Shared.NavMenu>();
        cut.Find(".navbar-toggler").Click();
        // After toggle the collapse class should be absent from the nav div
        var allDivs = cut.FindAll("div");
        var collapsedDiv = allDivs.FirstOrDefault(d =>
            (d.ClassName ?? "").Contains("collapse") && d.QuerySelector("nav") != null);
        Assert.Null(collapsedDiv);
    }

    [Fact]
    public void SharedNavMenu_ContainsHomeNavLink()
    {
        var cut = Render<DotDev.Client.Shared.NavMenu>();
        var links = cut.FindAll("a.nav-link");
        Assert.Contains(links, a => a.TextContent.Contains("Home"));
    }
}
