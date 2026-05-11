using DotDev.Core.HexPath;
using Xunit;

namespace Core.Tests;

public class StatusHexTests
{
    [Fact]
    public void StatusHex_WithEnabledTrue_IsNotGhost()
    {
        // Arrange
        var location = new HexLocation { Row = 1, Column = 1 };

        // Act
        var hex = new StatusHex(location, enable: true);

        // Assert
        Assert.False(hex.IsGhost);
    }

    [Fact]
    public void StatusHex_WithEnabledFalse_IsGhost()
    {
        // Arrange
        var location = new HexLocation { Row = 1, Column = 1 };

        // Act
        var hex = new StatusHex(location, enable: false);

        // Assert
        Assert.True(hex.IsGhost);
    }

    [Fact]
    public void StatusHex_HasDefaultStyle()
    {
        // Arrange & Act
        var defaultStyle = StatusHex.DefaultStyle;

        // Assert
        Assert.NotNull(defaultStyle);
        Assert.Equal("hexstatus", defaultStyle.HexClass);
    }

    [Fact]
    public void StatusHex_HasDefaultLocation()
    {
        // Arrange & Act
        var defaultLocation = StatusHex.DefaultLocation;

        // Assert
        Assert.Equal((3u, 5u), defaultLocation);
    }

    [Fact]
    public void StatusHex_HasLocationSet()
    {
        // Arrange
        var location = new HexLocation { Row = 1, Column = 1 };

        // Act
        var hex = new StatusHex(location, enable: true);

        // Assert
        Assert.NotNull(hex.Location);
        Assert.Equal(location.GridIndex, hex.Location.GridIndex);
    }

    [Fact]
    public void StatusHex_HasStyleSet()
    {
        // Arrange
        var location = new HexLocation { Row = 1, Column = 1 };

        // Act
        var hex = new StatusHex(location, enable: true);

        // Assert
        Assert.NotNull(hex.Style);
    }

    [Fact]
    public void StatusHex_DefaultStyle_ContainsDefaultImage()
    {
        // Arrange & Act
        var defaultStyle = StatusHex.DefaultStyle;

        // Assert
        Assert.NotNull(defaultStyle.Image);
        Assert.Contains("hex_placeholder", defaultStyle.Image);
    }

    [Fact]
    public void StatusHex_DefaultStyle_ContainsDefaultShade()
    {
        // Arrange & Act
        var defaultStyle = StatusHex.DefaultStyle;

        // Assert
        Assert.NotNull(defaultStyle.Shade);
        // Should be a hex color or similar
        Assert.True(defaultStyle.Shade.StartsWith("#") || defaultStyle.Shade.Contains("color"));
    }
}

public class AboutHexTests
{
    [Fact]
    public void AboutHex_WithEnabledTrue_IsNotGhost()
    {
        // Arrange
        var location = new HexLocation { Row = 1, Column = 1 };

        // Act
        var hex = new AboutHex(location, enable: true);

        // Assert
        Assert.False(hex.IsGhost);
    }

    [Fact]
    public void AboutHex_WithEnabledFalse_IsGhost()
    {
        // Arrange
        var location = new HexLocation { Row = 1, Column = 1 };

        // Act
        var hex = new AboutHex(location, enable: false);

        // Assert
        Assert.True(hex.IsGhost);
    }

    [Fact]
    public void AboutHex_HasDefaultStyle()
    {
        // Arrange & Act
        var defaultStyle = AboutHex.DefaultStyle;

        // Assert
        Assert.NotNull(defaultStyle);
        Assert.Equal("hexabout", defaultStyle.HexClass);
    }

    [Fact]
    public void AboutHex_HasDefaultLocation()
    {
        // Arrange & Act
        var defaultLocation = AboutHex.DefaultLocation;

        // Assert
        Assert.Equal((3u, 4u), defaultLocation);
    }

    [Fact]
    public void AboutHex_HasDefaultText()
    {
        // Arrange & Act
        var defaultText = AboutHex.DefaultText;

        // Assert
        Assert.NotNull(defaultText);
        Assert.NotEmpty(defaultText);
        Assert.Contains("About this site:", defaultText);
    }

    [Fact]
    public void AboutHex_WithEnabledTrue_HasConsoleText()
    {
        // Arrange
        var location = new HexLocation { Row = 1, Column = 1 };

        // Act
        var hex = new AboutHex(location, enable: true);

        // Assert
        Assert.NotNull(hex.ConsoleText);
        Assert.NotEmpty(hex.ConsoleText);
    }
}

public class IntroHexTests
{
    [Fact]
    public void IntroHex_IsCreated()
    {
        // Arrange
        var location = new HexLocation { Row = 0, Column = 0 };

        // Act
        var hex = new IntroHex(location);

        // Assert
        Assert.NotNull(hex);
    }

    [Fact]
    public void IntroHex_IsNotGhost()
    {
        // Arrange
        var location = new HexLocation { Row = 0, Column = 0 };

        // Act
        var hex = new IntroHex(location);

        // Assert
        Assert.False(hex.IsGhost);
    }

    [Fact]
    public void IntroHex_HasDefaultStyle()
    {
        // Arrange & Act
        var defaultStyle = IntroHex.DefaultStyle;

        // Assert
        Assert.NotNull(defaultStyle);
        Assert.False(defaultStyle.IsGhost);
        Assert.Equal("hexroot", defaultStyle.HexClass);
    }

    [Fact]
    public void IntroHex_HasDefaultLocation()
    {
        // Arrange & Act
        var defaultLocation = IntroHex.DefaultLocation;

        // Assert
        Assert.Equal((2u, 5u), defaultLocation);
    }

    [Fact]
    public void IntroHex_HasDefaultText()
    {
        // Arrange & Act
        var defaultText = IntroHex.DefaultText;

        // Assert
        Assert.NotNull(defaultText);
        Assert.NotEmpty(defaultText);
        Assert.Contains("Welcome...initializing...", defaultText);
    }

    [Fact]
    public void IntroHex_HasUnlocks()
    {
        // Arrange
        var location = new HexLocation { Row = 0, Column = 0 };

        // Act
        var hex = new IntroHex(location);

        // Assert
        Assert.NotNull(hex.Unlocks);
        Assert.NotEmpty(hex.Unlocks);
    }

    [Fact]
    public void IntroHex_UnlocksAboutAndStatus()
    {
        // Arrange
        var location = new HexLocation { Row = 0, Column = 0 };

        // Act
        var hex = new IntroHex(location);

        // Assert
        var unlocks = hex.Unlocks;
        Assert.Contains(unlocks, u => u.Item2 == HexOrder.About);
        Assert.Contains(unlocks, u => u.Item2 == HexOrder.Status);
    }

    [Fact]
    public void IntroHex_SetsLocationAndStyle()
    {
        // Arrange
        var location = new HexLocation { Row = 0, Column = 0 };

        // Act
        var hex = new IntroHex(location);

        // Assert
        Assert.NotNull(hex.Location);
        Assert.NotNull(hex.Style);
    }
}

public class BlankItemTests
{
    [Fact]
    public void BlankItem_IsCreated()
    {
        // Arrange
        var location = new HexLocation { Row = 1, Column = 1 };
        var style = new HexStyle();

        // Act
        var blank = new BlankItem(location, style);

        // Assert
        Assert.NotNull(blank);
    }

    [Fact]
    public void BlankItem_HasLocationSet()
    {
        // Arrange
        var location = new HexLocation { Row = 1, Column = 1 };
        var style = new HexStyle();

        // Act
        var blank = new BlankItem(location, style);

        // Assert
        Assert.NotNull(blank.Location);
    }

    [Fact]
    public void BlankItem_GridIndex_MatchesLocation()
    {
        // Arrange
        var location = new HexLocation { Row = 1, Column = 1 };
        var style = new HexStyle();

        // Act
        var blank = new BlankItem(location, style);

        // Assert
        Assert.Equal(location.GridIndex, blank.GridIndex);
    }
}
