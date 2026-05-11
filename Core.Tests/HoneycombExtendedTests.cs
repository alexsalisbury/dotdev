using DotDev.Core.HexPath;
using Xunit;

namespace Core.Tests;

public class HoneycombExtendedTests
{
    private Honeycomb MakeHoneycombWithRoot()
    {
        var hc = new Honeycomb();
        hc.AddRoot(new HexLocation(HexOrder.Intro, 2u, 5u));
        return hc;
    }

    [Fact]
    public async Task AddGhosts_OnIntroHex_AddsAboutAndStatusGhosts()
    {
        var hc = MakeHoneycombWithRoot();
        var introLoc = new HexLocation(HexOrder.Intro, 2u, 5u);

        await hc.AddGhosts(introLoc);

        var items = hc.GetItems().ToList();
        Assert.True(items.Count > 1, "AddGhosts should add ghost children of IntroHex");
        Assert.Contains(items, i => i is AboutHex);
        Assert.Contains(items, i => i is StatusHex);
    }

    [Fact]
    public async Task AddGhosts_OnIntroHex_GhostsAreGhost()
    {
        var hc = MakeHoneycombWithRoot();
        var introLoc = new HexLocation(HexOrder.Intro, 2u, 5u);

        await hc.AddGhosts(introLoc);

        var ghosts = hc.GetItems().Where(i => !(i is IntroHex)).ToList();
        Assert.All(ghosts, g => Assert.True(g.IsGhost));
    }

    [Fact]
    public async Task EnableGhosts_OnIntroHex_EnablesChildren()
    {
        var hc = MakeHoneycombWithRoot();
        var introLoc = new HexLocation(HexOrder.Intro, 2u, 5u);
        await hc.AddGhosts(introLoc);

        await hc.EnableGhosts(introLoc);

        var nonRoot = hc.GetItems().Where(i => !(i is IntroHex)).ToList();
        Assert.All(nonRoot, i => Assert.False(i.IsGhost));
    }

    [Fact]
    public async Task EnableGhosts_RaisesHoneycombChangedForEachEnabledItem()
    {
        var hc = MakeHoneycombWithRoot();
        var introLoc = new HexLocation(HexOrder.Intro, 2u, 5u);
        await hc.AddGhosts(introLoc);

        var eventCount = 0;
        hc.HoneycombChanged += (_, _) => eventCount++;

        await hc.EnableGhosts(introLoc);

        Assert.True(eventCount >= 2, "Should fire at least once per enabled child");
    }

    [Fact]
    public async Task AddGhosts_CalledTwice_DoesNotDuplicateItems()
    {
        var hc = MakeHoneycombWithRoot();
        var introLoc = new HexLocation(HexOrder.Intro, 2u, 5u);

        await hc.AddGhosts(introLoc);
        var countAfterFirst = hc.GetItems().Count();

        await hc.AddGhosts(introLoc);
        var countAfterSecond = hc.GetItems().Count();

        Assert.Equal(countAfterFirst, countAfterSecond);
    }

    [Fact]
    public async Task UnlockAsync_DoesNotThrow()
    {
        var hc = MakeHoneycombWithRoot();
        var exception = await Record.ExceptionAsync(() => hc.UnlockAsync());
        Assert.Null(exception);
    }

    [Fact]
    public void UnlockAsync_ReturnsTask()
    {
        var hc = MakeHoneycombWithRoot();
        var task = hc.UnlockAsync();
        Assert.NotNull(task);
    }

    [Fact]
    public async Task AddGhosts_OnIntroHex_AboutHexHasCorrectLocation()
    {
        var hc = MakeHoneycombWithRoot();
        var introLoc = new HexLocation(HexOrder.Intro, 2u, 5u);

        await hc.AddGhosts(introLoc);

        var about = hc.GetItems().OfType<AboutHex>().FirstOrDefault();
        Assert.NotNull(about);
        Assert.Equal(HexOrder.About, about.Location.Index);
    }

    [Fact]
    public async Task AddGhosts_OnIntroHex_StatusHexHasCorrectLocation()
    {
        var hc = MakeHoneycombWithRoot();
        var introLoc = new HexLocation(HexOrder.Intro, 2u, 5u);

        await hc.AddGhosts(introLoc);

        var status = hc.GetItems().OfType<StatusHex>().FirstOrDefault();
        Assert.NotNull(status);
        Assert.Equal(HexOrder.Status, status.Location.Index);
    }

    [Fact]
    public async Task EnableGhosts_WithoutPriorAddGhosts_AddsEnabledItems()
    {
        var hc = MakeHoneycombWithRoot();
        var introLoc = new HexLocation(HexOrder.Intro, 2u, 5u);

        // EnableGhosts without a prior AddGhosts still creates and adds enabled items
        var result = await hc.EnableGhosts(introLoc);
        Assert.True(result);
        Assert.True(hc.GetItems().Count() > 1);
    }
}
