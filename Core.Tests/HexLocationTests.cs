using DotDev.Core.HexPath;
using Xunit;

namespace Core.Tests;

public class HexLocationTests
{
    [Fact]
    public void HexLocation_Constructor_SetsProperties()
    {
        // Arrange & Act
        var location = new HexLocation { Row = 5, Column = 10 };

        // Assert
        Assert.Equal((uint)510, location.GridIndex);
        Assert.Equal((uint)5, location.Row);
        Assert.Equal((uint)10, location.Column);
    }

    [Fact]
    public void HexLocation_GridIndex_Equality()
    {
        // Arrange
        var location1 = new HexLocation { Row = 5, Column = 10 };
        var location2 = new HexLocation { Row = 5, Column = 10 };

        // Act & Assert
        Assert.Equal(location1.GridIndex, location2.GridIndex);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    [InlineData(4, 3)]
    [InlineData(5, 4)]
    [InlineData(6, 5)]
    public void HexLocation_Move_WithValidDirection_ReturnsNewLocation(uint direction, uint hexOrder)
    {
        // Arrange
        var location = new HexLocation { Row = 5, Column = 5 };
        var hexOrderValue = (HexOrder)hexOrder;

        // Act
        var newLocation = location.Move(direction, hexOrderValue);

        // Assert
        Assert.NotNull(newLocation);
        Assert.NotEqual(location.GridIndex, newLocation.GridIndex);
    }

    [Fact]
    public void HexLocation_Move_PreservesMovedProperties()
    {
        // Arrange
        var location = new HexLocation { Row = 5, Column = 5 };

        // Act
        var newLocation = location.Move(1, HexOrder.Status);

        // Assert
        Assert.NotNull(newLocation);
        Assert.IsType<HexLocation>(newLocation);
    }

    [Fact]
    public void HexLocation_RowAndColumn_AreUnsignedIntegers()
    {
        // Arrange
        var location = new HexLocation { Row = 100, Column = 200 };

        // Act & Assert
        Assert.IsType<uint>(location.Row);
        Assert.IsType<uint>(location.Column);
        Assert.Equal((uint)100, location.Row);
        Assert.Equal((uint)200, location.Column);
    }

    [Fact]
    public void HexLocation_WithZeroCoordinates_IsValid()
    {
        // Arrange & Act
        var location = new HexLocation { Row = 0, Column = 0 };

        // Assert
        Assert.Equal((uint)0, location.GridIndex);
        Assert.Equal((uint)0, location.Row);
        Assert.Equal((uint)0, location.Column);
    }

    [Fact]
    public void HexLocation_WithLargeCoordinates_IsValid()
    {
        // Arrange
        var largeValue = uint.MaxValue;

        // Act
        var location = new HexLocation { Row = largeValue, Column = largeValue };

        // Assert
        //Assert.Equal(largeValue, location.GridIndex); LOLOLOLOLOL
        Assert.Equal(largeValue, location.Row);
        Assert.Equal(largeValue, location.Column);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(101, 1, 1)]
    [InlineData(510, 5, 10)]
    public void HexLocation_MultipleProperties_CanBeSetIndependently(uint gridIndex, uint row, uint column)
    {
        // Arrange & Act
        var location = new HexLocation { Row = row, Column = column };

        // Assert
        Assert.Equal(gridIndex, location.GridIndex);
        Assert.Equal(row, location.Row);
        Assert.Equal(column, location.Column);
    }
}
