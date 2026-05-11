using DotDev.Core.HexPath;
using Xunit;

namespace Core.Tests;

public class HexLocationExtendedTests
{
    [Fact]
    public void HexLocation_TupleConstructor_SetsIndexAndCoordinates()
    {
        var loc = new HexLocation(HexOrder.Intro, (2u, 5u));
        Assert.Equal(HexOrder.Intro, loc.Index);
        Assert.Equal(2u, loc.Row);
        Assert.Equal(5u, loc.Column);
    }

    [Fact]
    public void HexLocation_ThreeParamConstructor_SetsIndexAndCoordinates()
    {
        var loc = new HexLocation(HexOrder.Status, 3u, 5u);
        Assert.Equal(HexOrder.Status, loc.Index);
        Assert.Equal(3u, loc.Row);
        Assert.Equal(5u, loc.Column);
    }

    [Fact]
    public void HexLocation_TupleAndThreeParam_ProduceSameGridIndex()
    {
        var a = new HexLocation(HexOrder.About, (3u, 4u));
        var b = new HexLocation(HexOrder.About, 3u, 4u);
        Assert.Equal(a.GridIndex, b.GridIndex);
    }

    // Move() direction 1: row-1, same column
    [Fact]
    public void HexLocation_Move_Direction1_DecrementsRow()
    {
        var loc = new HexLocation { Row = 5, Column = 5 };
        var moved = loc.Move(1, HexOrder.About);
        Assert.Equal(4u, moved.Row);
        Assert.Equal(5u, moved.Column);
    }

    // Move() direction 2: row-1, column+1
    [Fact]
    public void HexLocation_Move_Direction2_DecrementsRowIncrementsColumn()
    {
        var loc = new HexLocation { Row = 5, Column = 5 };
        var moved = loc.Move(2, HexOrder.About);
        Assert.Equal(4u, moved.Row);
        Assert.Equal(6u, moved.Column);
    }

    // Move() direction 3: same row, column+1
    [Fact]
    public void HexLocation_Move_Direction3_SameRowIncrementsColumn()
    {
        var loc = new HexLocation { Row = 5, Column = 5 };
        var moved = loc.Move(3, HexOrder.About);
        Assert.Equal(5u, moved.Row);
        Assert.Equal(6u, moved.Column);
    }

    // Move() direction 4: row+1, same column
    [Fact]
    public void HexLocation_Move_Direction4_IncrementsRow()
    {
        var loc = new HexLocation { Row = 5, Column = 5 };
        var moved = loc.Move(4, HexOrder.About);
        Assert.Equal(6u, moved.Row);
        Assert.Equal(5u, moved.Column);
    }

    // Move() direction 5: same row, column-1
    [Fact]
    public void HexLocation_Move_Direction5_SameRowDecrementsColumn()
    {
        var loc = new HexLocation { Row = 5, Column = 5 };
        var moved = loc.Move(5, HexOrder.About);
        Assert.Equal(5u, moved.Row);
        Assert.Equal(4u, moved.Column);
    }

    // Move() direction 0 (default): row+1, column-1
    [Fact]
    public void HexLocation_Move_DefaultDirection_IncrementsRowDecrementsColumn()
    {
        var loc = new HexLocation { Row = 5, Column = 5 };
        var moved = loc.Move(0, HexOrder.About);
        Assert.Equal(6u, moved.Row);
        Assert.Equal(4u, moved.Column);
    }

    // Move() direction 6+ also hits default
    [Fact]
    public void HexLocation_Move_Direction6_IncrementsRowDecrementsColumn()
    {
        var loc = new HexLocation { Row = 5, Column = 5 };
        var moved = loc.Move(6, HexOrder.About);
        Assert.Equal(6u, moved.Row);
        Assert.Equal(4u, moved.Column);
    }

    [Fact]
    public void HexLocation_Move_SetsTargetIndex()
    {
        var loc = new HexLocation { Row = 5, Column = 5 };
        var moved = loc.Move(3, HexOrder.Status);
        Assert.Equal(HexOrder.Status, moved.Index);
    }

    [Fact]
    public void HexLocation_Move_ReturnsNewInstance()
    {
        var loc = new HexLocation { Row = 5, Column = 5 };
        var moved = loc.Move(1, HexOrder.About);
        Assert.NotSame(loc, moved);
    }

    [Fact]
    public void HexLocation_Move_OriginalIsUnchanged()
    {
        var loc = new HexLocation { Row = 5, Column = 5 };
        loc.Move(1, HexOrder.About);
        Assert.Equal(5u, loc.Row);
        Assert.Equal(5u, loc.Column);
    }
}
