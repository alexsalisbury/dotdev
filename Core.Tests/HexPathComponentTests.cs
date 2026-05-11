using Bunit;
using DotDev.Client.Shared.HexPath;
using DotDev.Core.HexPath;
using Moq;
using Xunit;

namespace Core.Tests;

public class HexPathComponentTests : TestContext
{
    private Mock<IHoneycomb> MakeMock()
    {
        var mock = new Mock<IHoneycomb>();
        mock.Setup(h => h.GetItems()).Returns(Array.Empty<HexItem>());
        mock.Setup(h => h.AddRoot(It.IsAny<HexLocation>())).Returns(true);
        mock.Setup(h => h.UnlockAsync()).Returns(Task.CompletedTask);
        return mock;
    }

    [Fact]
    public void HexPath_RendersWithoutCrash()
    {
        var mock = MakeMock();
        var cut = Render<HexPath>(p => p.Add(c => c.Grid, mock.Object));
        Assert.NotNull(cut);
    }

    [Fact]
    public void HexPath_ContainsHexPathDiv()
    {
        var mock = MakeMock();
        var cut = Render<HexPath>(p => p.Add(c => c.Grid, mock.Object));
        var div = cut.Find(".hexPath");
        Assert.NotNull(div);
    }

    [Fact]
    public void HexPath_OnInitialized_CallsAddRoot()
    {
        var mock = MakeMock();
        Render<HexPath>(p => p.Add(c => c.Grid, mock.Object));
        mock.Verify(h => h.AddRoot(It.IsAny<HexLocation>()), Times.Once);
    }

    [Fact]
    public void HexPath_OnInitialized_SubscribesToHoneycombChanged()
    {
        var mock = MakeMock();
        Render<HexPath>(p => p.Add(c => c.Grid, mock.Object));
        mock.VerifyAdd(h => h.HoneycombChanged += It.IsAny<EventHandler<EventArgs>>(), Times.Once);
    }

    [Fact]
    public async Task HexPath_StepUnlockAsync_CallsGridUnlockAsync()
    {
        var mock = MakeMock();
        var cut = Render<HexPath>(p => p.Add(c => c.Grid, mock.Object));

        await cut.Instance.StepUnlockAsync();

        mock.Verify(h => h.UnlockAsync(), Times.Once);
    }

    [Fact]
    public void HexPath_AddRoot_CalledWithIntroLocation()
    {
        var mock = MakeMock();
        Render<HexPath>(p => p.Add(c => c.Grid, mock.Object));
        mock.Verify(
            h => h.AddRoot(It.Is<HexLocation>(loc => loc.Index == HexOrder.Intro)),
            Times.Once);
    }

    [Fact]
    public void HexPath_RendersOneHexPerItem()
    {
        var mock = MakeMock();
        mock.Setup(h => h.GetItems()).Returns(Array.Empty<HexItem>());
        var cut = Render<HexPath>(p => p.Add(c => c.Grid, mock.Object));
        Assert.Empty(cut.FindAll(".hexPath > *"));
    }
}
