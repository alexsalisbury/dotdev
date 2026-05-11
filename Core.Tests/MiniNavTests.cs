using Bunit;
using DotDev.Client.Shared;
using Xunit;

namespace Core.Tests;

public class MiniNavTests : TestContext
{
    [Fact]
    public void MiniNav_RendersWithoutCrash()
    {
        var cut = Render<MiniNav>();
        Assert.NotNull(cut);
    }

    [Fact]
    public void MiniNav_ContainsSocialDiv()
    {
        var cut = Render<MiniNav>();
        var div = cut.Find(".social");
        Assert.NotNull(div);
    }

    [Fact]
    public void MiniNav_ContainsSpotifyLink()
    {
        var cut = Render<MiniNav>();
        var links = cut.FindAll("a");
        Assert.Contains(links, a => a.GetAttribute("href")?.Contains("spotify") == true);
    }

    [Fact]
    public void MiniNav_ContainsTwitterLink()
    {
        var cut = Render<MiniNav>();
        var links = cut.FindAll("a");
        Assert.Contains(links, a => a.GetAttribute("href")?.Contains("twitter") == true);
    }

    [Fact]
    public void MiniNav_ContainsInstagramLink()
    {
        var cut = Render<MiniNav>();
        var links = cut.FindAll("a");
        Assert.Contains(links, a => a.GetAttribute("href")?.Contains("instagram") == true);
    }

    [Fact]
    public void MiniNav_ContainsGitHubLink()
    {
        var cut = Render<MiniNav>();
        var links = cut.FindAll("a");
        Assert.Contains(links, a => a.GetAttribute("href")?.Contains("github") == true);
    }

    [Fact]
    public void MiniNav_HasFourSocialLinks()
    {
        var cut = Render<MiniNav>();
        var links = cut.FindAll(".social a");
        Assert.Equal(4, links.Count);
    }

    [Fact]
    public void MiniNav_SocialLinks_HaveImages()
    {
        var cut = Render<MiniNav>();
        var images = cut.FindAll(".social a img");
        Assert.Equal(4, images.Count);
    }
}
