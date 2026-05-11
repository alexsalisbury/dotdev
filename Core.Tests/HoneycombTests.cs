using DotDev.Core.HexPath;
using Moq;
using Xunit;

namespace Core.Tests;

public class HoneycombTests
{
    private readonly Honeycomb _honeycomb = new();

    [Fact]
    public void AddItem_WithEmptyHexMap_AddsItemSuccessfully()
    {
        // Arrange
        var location = new HexLocation { Row = 1, Column = 1 };
        var item = new StatusHex(location, enable: true);

        // Act
        var result = _honeycomb.AddItem(item);

        // Assert
        Assert.True(result);
        Assert.Contains(item, _honeycomb.GetItems());
    }

    [Fact]
    public void AddItem_WithEmptyHexMap_RaisesHoneycombChangedEvent()
    {
        // Arrange
        var location = new HexLocation { Row = 1, Column = 1 };
        var item = new StatusHex(location, enable: true);
        var eventRaised = false;

        _honeycomb.HoneycombChanged += (_, _) => eventRaised = true;

        // Act
        _honeycomb.AddItem(item);

        // Assert
        Assert.True(eventRaised);
    }

    [Fact]
    public void AddItem_GhostCannotReplaceExistingItem()
    {
        // Arrange
        var location = new HexLocation { Row = 1, Column = 1 };
        var existingItem = new StatusHex(location, enable: true);

        var ghostItem = new StatusHex(location, enable: false);

        _honeycomb.AddItem(existingItem);

        // Act
        var result = _honeycomb.AddItem(ghostItem);

        // Assert
        Assert.False(result);
        Assert.DoesNotContain(ghostItem, _honeycomb.GetItems());
    }

    [Fact]
    public void AddItem_NonGhostReplacesGhost()
    {
        // Arrange
        var location = new HexLocation { Row = 1, Column = 1 };
        var ghostItem = new StatusHex(location, enable: false);

        var nonGhostItem = new StatusHex(location, enable: true);

        _honeycomb.AddItem(ghostItem);

        // Act
        var result = _honeycomb.AddItem(nonGhostItem);

        // Assert
        Assert.True(result);
        Assert.Contains(nonGhostItem, _honeycomb.GetItems());
        Assert.DoesNotContain(ghostItem, _honeycomb.GetItems());
    }

    [Fact]
    public void AddItem_NonGhostReplacesGhost_RaisesHoneycombChangedEvent()
    {
        // Arrange
        var location = new HexLocation { Row = 1, Column = 1 };
        var ghostItem = new StatusHex(location, enable: false);

        var nonGhostItem = new StatusHex(location, enable: true);

        _honeycomb.AddItem(ghostItem);
        var eventCount = 0;
        _honeycomb.HoneycombChanged += (_, _) => eventCount++;

        // Act
        _honeycomb.AddItem(nonGhostItem);

        // Assert
        Assert.Equal(1, eventCount);
    }

    [Fact]
    public void AddItem_NonGhostCannotReplaceNonGhost()
    {
        // Arrange
        var location = new HexLocation { Row = 1, Column = 1 };
        var firstItem = new StatusHex(location, enable: true);
        var secondItem = new StatusHex(location, enable: true);

        _honeycomb.AddItem(firstItem);

        // Act
        var result = _honeycomb.AddItem(secondItem);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AddItem_GhostCannotReplaceGhost()
    {
        // Arrange
        var location = new HexLocation { Row = 1, Column = 1 };
        var firstGhost = new StatusHex(location, enable: false);
        var secondGhost = new StatusHex(location, enable: false);

        _honeycomb.AddItem(firstGhost);

        // Act
        var result = _honeycomb.AddItem(secondGhost);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AddRoot_CreatesIntroHexAndAddsIt()
    {
        // Arrange
        var location = new HexLocation { Row = 0, Column = 0 };

        // Act
        var result = _honeycomb.AddRoot(location);

        // Assert
        Assert.True(result);
        var items = _honeycomb.GetItems();
        Assert.Single(items);
        Assert.IsType<IntroHex>(items.First());
    }

    //[Fact]
    //public void AddRoot_WithExistingRoot_CannotReplace()
    //{
    //    // Arrange
    //    var location1 = new HexLocation { Row = 0, Column = 0 };
    //    var location2 = new HexLocation { Row = 1, Column = 1 };

    //    _honeycomb.AddRoot(location1);

    //    // Act
    //    var result = _honeycomb.AddRoot(location2);

    //    // Assert
    //    Assert.False(result);
    //}

    [Fact]
    public async Task AddGhosts_WithSourceItem_AddsGhostItems()
    {
        // Arrange
        var location = new HexLocation { Row = 1, Column = 1 };
        var item = new StatusHex(location, enable: true);
        _honeycomb.AddItem(item);

        // Act
        var result = await _honeycomb.AddGhosts(location);

        // Assert
        Assert.True(result);
        // StatusHex should have generated ghosts based on its unlocks
        Assert.NotEmpty(_honeycomb.GetItems());
    }

    [Fact]
    public void GetItems_ReturnsAllAddedItems()
    {
        // Arrange
        var location1 = new HexLocation { Row = 1, Column = 1 };
        var location2 = new HexLocation { Row = 1, Column = 2 };

        var item1 = new StatusHex(location1, enable: true);
        var item2 = new StatusHex(location2, enable: true);

        _honeycomb.AddItem(item1);
        _honeycomb.AddItem(item2);

        // Act
        var items = _honeycomb.GetItems();

        // Assert
        Assert.Equal(2, items.Count());
    }

    [Fact]
    public void GetItems_WithEmptyHoneycomb_ReturnsEmpty()
    {
        // Act
        var items = _honeycomb.GetItems();

        // Assert
        Assert.Empty(items);
    }

    [Fact]
    public void MultipleAddItem_DoesNotDuplicateInResults()
    {
        // Arrange
        var location = new HexLocation { Row = 1, Column = 1 };
        var item = new StatusHex(location, enable: true);

        // Act
        _honeycomb.AddItem(item);
        var items1 = _honeycomb.GetItems().Count();

        _honeycomb.AddItem(item);
        var items2 = _honeycomb.GetItems().Count();

        // Assert
        Assert.Equal(1, items1);
        Assert.Equal(1, items2);
    }

    [Fact]
    public void AddItem_WithDifferentGridIndexes_BothAdded()
    {
        // Arrange
        var location1 = new HexLocation { Row = 1, Column = 1 };
        var location2 = new HexLocation { Row = 1, Column = 2 };

        var item1 = new StatusHex(location1, enable: true);
        var item2 = new StatusHex(location2, enable: true);

        // Act
        var result1 = _honeycomb.AddItem(item1);
        var result2 = _honeycomb.AddItem(item2);

        // Assert
        Assert.True(result1);
        Assert.True(result2);
        Assert.Equal(2, _honeycomb.GetItems().Count());
    }

    [Fact]
    public void HoneycombChanged_IsNullWhenNoSubscribers()
    {
        // Arrange & Act
        var location = new HexLocation { Row = 1, Column = 1 };
        var item = new StatusHex(location, enable: true);

        // Should not throw even with no subscribers
        var result = _honeycomb.AddItem(item);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AddItem_MultipleSubscribers_AllReceiveEvent()
    {
        // Arrange
        var location = new HexLocation { Row = 1, Column = 1 };
        var item = new StatusHex(location, enable: true);

        var eventCount = 0;
        _honeycomb.HoneycombChanged += (_, _) => eventCount++;
        _honeycomb.HoneycombChanged += (_, _) => eventCount++;

        // Act
        _honeycomb.AddItem(item);

        // Assert
        Assert.Equal(2, eventCount);
    }
}
