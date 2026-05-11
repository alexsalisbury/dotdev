using Bunit;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Core.Tests;

public class LayoutMainLayoutTests : TestContext
{
    private IRenderedComponent<DotDev.Client.Layout.MainLayout> RenderLayout(string bodyContent = "test body")
    {
        return Render<DotDev.Client.Layout.MainLayout>(p =>
            p.Add(c => c.Body, b => b.AddContent(0, bodyContent)));
    }

    [Fact]
    public void LayoutMainLayout_RendersWithoutCrash()
    {
        var cut = RenderLayout();
        Assert.NotNull(cut);
    }

    [Fact]
    public void LayoutMainLayout_ContainsPageDiv()
    {
        var cut = RenderLayout();
        var page = cut.Find(".page");
        Assert.NotNull(page);
    }

    [Fact]
    public void LayoutMainLayout_ContainsSidebar()
    {
        var cut = RenderLayout();
        var sidebar = cut.Find(".sidebar");
        Assert.NotNull(sidebar);
    }

    [Fact]
    public void LayoutMainLayout_ContainsMainElement()
    {
        var cut = RenderLayout();
        var main = cut.Find("main");
        Assert.NotNull(main);
    }

    [Fact]
    public void LayoutMainLayout_RendersBodyContent()
    {
        var cut = RenderLayout("hello from body");
        Assert.Contains("hello from body", cut.Markup);
    }

    [Fact]
    public void LayoutMainLayout_ContainsNavMenu()
    {
        var cut = RenderLayout();
        var navbar = cut.Find(".navbar-brand");
        Assert.NotNull(navbar);
    }

    [Fact]
    public void LayoutMainLayout_ContainsContentArticle()
    {
        var cut = RenderLayout();
        var article = cut.Find("article.content");
        Assert.NotNull(article);
    }
}

public class SharedMainLayoutTests : TestContext
{
    private IRenderedComponent<DotDev.Client.Shared.MainLayout> RenderLayout(string bodyContent = "test body")
    {
        return Render<DotDev.Client.Shared.MainLayout>(p =>
            p.Add(c => c.Body, b => b.AddContent(0, bodyContent)));
    }

    [Fact]
    public void SharedMainLayout_RendersWithoutCrash()
    {
        var cut = RenderLayout();
        Assert.NotNull(cut);
    }

    [Fact]
    public void SharedMainLayout_ContainsPageDiv()
    {
        var cut = RenderLayout();
        var page = cut.Find(".page");
        Assert.NotNull(page);
    }

    [Fact]
    public void SharedMainLayout_ContainsMainElement()
    {
        var cut = RenderLayout();
        var main = cut.Find("main");
        Assert.NotNull(main);
    }

    [Fact]
    public void SharedMainLayout_ContainsHeaderElement()
    {
        var cut = RenderLayout();
        var header = cut.Find("header");
        Assert.NotNull(header);
    }

    [Fact]
    public void SharedMainLayout_RendersBodyContent()
    {
        var cut = RenderLayout("shared body content");
        Assert.Contains("shared body content", cut.Markup);
    }

    [Fact]
    public void SharedMainLayout_ContainsContentArticle()
    {
        var cut = RenderLayout();
        var article = cut.Find("article.content");
        Assert.NotNull(article);
    }
}
