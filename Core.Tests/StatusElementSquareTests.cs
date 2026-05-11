using Bunit;
using DotDev.Client.Shared.Status;
using DotDev.Core.Element;
using Xunit;

namespace Core.Tests;

public class StatusElementSquareTests : TestContext
{
    private static ElementInfo MakeElement(int number = 1, int column = 2, int row = 3) =>
        new ElementInfo(number, "H", "Hydrogen", "1.008", "[1]", "other-nonmetal", column, row);

    private static Square MakeSquare(ElementInfo? info = null, ServerInfo? status = null) =>
        new Square(info ?? MakeElement(), status);

    [Fact]
    public void StatusElementSquare_RendersWithoutCrash()
    {
        var sq = MakeSquare();
        var cut = Render<StatusElementSquare>(p => p.Add(c => c.SQ, sq));
        Assert.NotNull(cut);
    }

    [Fact]
    public void StatusElementSquare_SquareClass_ContainsSquarePrefix()
    {
        var sq = MakeSquare();
        var cut = Render<StatusElementSquare>(p => p.Add(c => c.SQ, sq));
        Assert.Contains("square", cut.Find("div").ClassName ?? string.Empty);
    }

    [Fact]
    public void StatusElementSquare_SquareClass_ContainsColumn()
    {
        var sq = MakeSquare(MakeElement(column: 5, row: 2));
        var cut = Render<StatusElementSquare>(p => p.Add(c => c.SQ, sq));
        Assert.Contains("c5", cut.Find("div").ClassName ?? string.Empty);
    }

    [Fact]
    public void StatusElementSquare_SquareClass_ContainsRow()
    {
        var sq = MakeSquare(MakeElement(column: 5, row: 2));
        var cut = Render<StatusElementSquare>(p => p.Add(c => c.SQ, sq));
        Assert.Contains("r2", cut.Find("div").ClassName ?? string.Empty);
    }

    [Fact]
    public void StatusElementSquare_ServerStatus_UnknownWhenNoServerInfo()
    {
        var sq = MakeSquare(status: null);
        var cut = Render<StatusElementSquare>(p => p.Add(c => c.SQ, sq));
        Assert.Equal("unknown", cut.Instance.ServerStatus);
    }

    [Fact]
    public void StatusElementSquare_ServerStatus_ActiveWhenRecentlySeen()
    {
        var info = MakeElement();
        var status = new ServerInfo
        {
            Number = 1,
            Name = "Hydrogen",
            Symbol = "H",
            LastStatus = "active",
            DeviceType = 1,
            LastSeen = DateTimeOffset.UtcNow.AddSeconds(-30)
        };
        var sq = new Square(info, status);
        var cut = Render<StatusElementSquare>(p => p.Add(c => c.SQ, sq));
        Assert.Equal("active", cut.Instance.ServerStatus);
    }

    [Fact]
    public void StatusElementSquare_ServerStatus_UntrackedWhenStatusIsUntracked()
    {
        var info = MakeElement();
        var status = new ServerInfo
        {
            Number = 1,
            Name = "Hydrogen",
            Symbol = "H",
            LastStatus = "untracked",
            DeviceType = 1,
            LastSeen = DateTimeOffset.UtcNow.AddSeconds(-30)
        };
        var sq = new Square(info, status);
        var cut = Render<StatusElementSquare>(p => p.Add(c => c.SQ, sq));
        Assert.Equal("untracked", cut.Instance.ServerStatus);
    }

    [Fact]
    public void StatusElementSquare_OverlayClass_IncludesServerStatus()
    {
        var info = MakeElement();
        var status = new ServerInfo
        {
            Number = 1,
            Name = "Hydrogen",
            Symbol = "H",
            LastStatus = "active",
            DeviceType = 1,
            LastSeen = DateTimeOffset.UtcNow.AddSeconds(-30)
        };
        var sq = new Square(info, status);
        var cut = Render<StatusElementSquare>(p => p.Add(c => c.SQ, sq));
        Assert.Contains("active", cut.Instance.OverlayClass);
        Assert.Contains("overlay", cut.Instance.OverlayClass);
    }

    [Fact]
    public async Task StatusElementSquare_DisposeAsync_CanBeCalledSafely()
    {
        var sq = MakeSquare();
        var cut = Render<StatusElementSquare>(p => p.Add(c => c.SQ, sq));

        var exception = await Record.ExceptionAsync(async () =>
        {
            await cut.Instance.DisposeAsync();
        });

        Assert.Null(exception);
    }

    [Fact]
    public async Task StatusElementSquare_DisposeAsync_UnsubscribesOnChange()
    {
        var sq = MakeSquare();
        var cut = Render<StatusElementSquare>(p => p.Add(c => c.SQ, sq));

        await cut.Instance.DisposeAsync();

        var exception = Record.Exception(() => sq.SetElement(MakeElement()));
        Assert.Null(exception);
    }

    [Fact]
    public void StatusElementSquare_SquareClass_IncludesNoPulseInitially()
    {
        var sq = MakeSquare(MakeElement(column: 3, row: 4));
        var cut = Render<StatusElementSquare>(p => p.Add(c => c.SQ, sq));
        var squareClass = cut.Instance.SquareClass;
        Assert.Equal("square c3 r4 ", squareClass);
    }
}
