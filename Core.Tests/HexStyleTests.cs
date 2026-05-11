using DotDev.Core.HexPath;
using Xunit;

namespace Core.Tests;

public class HexStyleTests
{
    [Fact]
    public void HexStyle_Constructor_CreatesDefaultInstance()
    {
        // Arrange & Act
        var style = new HexStyle();

        // Assert
        Assert.NotNull(style);
        Assert.False(style.IsGhost);
    }

    [Fact]
    public void HexStyle_Size_ReturnsConstantValue()
    {
        // Arrange
        var style = new HexStyle();

        // Act
        var size = style.Size;

        // Assert
        Assert.Equal("100", size);
    }

    [Fact]
    public void HexStyle_ViewBox_ContainsSize()
    {
        // Arrange
        var style = new HexStyle();

        // Act
        var viewBox = style.ViewBox;

        // Assert
        Assert.Equal("0 0 100 100", viewBox);
    }

    [Fact]
    public void HexStyle_Points_GeneratesHexagonVertices()
    {
        // Arrange
        var style = new HexStyle();

        // Act
        var points = style.Points;

        // Assert
        Assert.NotNull(points);
        Assert.NotEmpty(points);
        // Should have 6 pairs of coordinates (6 vertices of hexagon)
        var pairs = points.Split(' ');
        Assert.Equal(7, pairs.Length); // 6 vertices + closing vertex
    }

    [Fact]
    public void HexStyle_Points_ContainsValidCoordinates()
    {
        // Arrange
        var style = new HexStyle();

        // Act
        var points = style.Points;

        // Assert
        Assert.NotNull(points);
        Assert.Contains(",", points); // coordinates should have commas
        var pairs = points.Split(' ');
        foreach (var pair in pairs)
        {
            var coords = pair.Split(',');
            Assert.Equal(2, coords.Length);
            Assert.True(double.TryParse(coords[0], out _), $"First coordinate of '{pair}' should be parseable");
            Assert.True(double.TryParse(coords[1], out _), $"Second coordinate of '{pair}' should be parseable");
        }
    }

    [Fact]
    public void HexStyle_MaxSizes_ContainsHeightAndWidth()
    {
        // Arrange
        var style = new HexStyle();

        // Act
        var maxSizes = style.MaxSizes;

        // Assert
        Assert.Contains("max-height", maxSizes);
        Assert.Contains("max-width", maxSizes);
        Assert.Contains("100px", maxSizes);
    }

    [Fact]
    public void HexStyle_IsGhost_CanBeSet()
    {
        // Arrange
        var style = new HexStyle { IsGhost = true };

        // Act & Assert
        Assert.True(style.IsGhost);
    }

    [Fact]
    public void HexStyle_Properties_CanBeSet()
    {
        // Arrange
        var style = new HexStyle
        {
            Shade = "#FF0000",
            HexClass = "test-class",
            Target = "_blank",
            Image = "test.png"
        };

        // Act & Assert
        Assert.Equal("#FF0000", style.Shade);
        Assert.Equal("test-class", style.HexClass);
        Assert.Equal("_blank", style.Target);
        Assert.Equal("test.png", style.Image);
    }

    [Fact]
    public void HexStyle_Properties_CanBeNull()
    {
        // Arrange
        var style = new HexStyle
        {
            Shade = null,
            HexClass = null,
            Target = null,
            Image = null
        };

        // Act & Assert
        Assert.Null(style.Shade);
        Assert.Null(style.HexClass);
        Assert.Null(style.Target);
        Assert.Null(style.Image);
    }

    [Fact]
    public void HexStyle_AsRecord_SupportsEquality()
    {
        // Arrange
        var style1 = new HexStyle { IsGhost = false, Shade = "#FF0000" };
        var style2 = new HexStyle { IsGhost = false, Shade = "#FF0000" };

        // Act & Assert
        Assert.Equal(style1, style2);
    }

    [Fact]
    public void HexStyle_Points_AreConsistent()
    {
        // Arrange
        var style = new HexStyle();

        // Act
        var points1 = style.Points;
        var points2 = style.Points;

        // Assert
        Assert.Equal(points1, points2);
    }

    [Fact]
    public void HexStyle_GeometricProperties_AreConsistent()
    {
        // Arrange
        var style = new HexStyle();

        // Act
        var size = style.Size;
        var viewBox = style.ViewBox;
        var maxSizes = style.MaxSizes;

        // Assert
        Assert.Equal("100", size);
        Assert.Equal("0 0 100 100", viewBox);
        Assert.Equal("max-height:100px;max-width:100px;", maxSizes);
    }
}
