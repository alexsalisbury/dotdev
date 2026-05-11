using DotDev.Core.Element;
using Xunit;

namespace Core.Tests;

public class SquareModelTests
{
    [Fact]
    public void Square_Constructor_SetsInfoAndStatus()
    {
        var info = new ElementInfo(1, "H", "Hydrogen", 1.008, "[1]", "other-nonmetal", 1, 1);
        var status = new ServerInfo
        {
            Number = 1,
            Symbol = "H",
            Name = "Hydrogen",
            LastStatus = "active",
            DeviceType = 1,
            LastSeen = DateTimeOffset.UtcNow
        };

        var square = new Square(info, status);

        Assert.Equal(info, square.Info);
        Assert.Equal(status, square.Status);
        Assert.True(square.Populated);
    }

    [Fact]
    public void Square_WithNullStatus_IsNotPopulated()
    {
        var info = new ElementInfo(1, "H", "Hydrogen", 1.008, "[1]", "other-nonmetal", 1, 1);
        var square = new Square(info, null);

        Assert.False(square.Populated);
        Assert.Equal("unknown", square.ServerStatusClass);
    }

    [Fact]
    public void Square_SetLastSeen_TriggersOnChange()
    {
        var info = new ElementInfo(1, "H", "Hydrogen", 1.008, "[1]", "other-nonmetal", 1, 1);
        var square = new Square(info, null);
        var eventFired = false;
        square.OnChange += (_, _) => eventFired = true;

        square.SetLastSeen(DateTimeOffset.UtcNow);

        Assert.True(eventFired);
    }

    [Fact]
    public void Square_SetLastSeen_WithNullStatus_CreatesNewServerInfo()
    {
        var info = new ElementInfo(1, "H", "Hydrogen", 1.008, "[1]", "other-nonmetal", 1, 1);
        var square = new Square(info, null);

        square.SetLastSeen(DateTimeOffset.UtcNow);

        Assert.NotNull(square.Status);
        Assert.Equal(1, square.Status.Number);
    }

    [Fact]
    public void Square_SetLastSeen_WithExistingStatus_UpdatesLastSeen()
    {
        var info = new ElementInfo(1, "H", "Hydrogen", 1.008, "[1]", "other-nonmetal", 1, 1);
        var originalTime = DateTimeOffset.UtcNow.AddMinutes(-10);
        var status = new ServerInfo
        {
            Number = 1,
            Symbol = "H",
            Name = "Hydrogen",
            LastStatus = "active",
            DeviceType = 1,
            LastSeen = originalTime
        };
        var square = new Square(info, status);
        var newTime = DateTimeOffset.UtcNow;

        square.SetLastSeen(newTime);

        Assert.Equal(newTime, square.Status?.LastSeen);
    }

    [Fact]
    public void Square_SetElement_TriggersOnChange()
    {
        var info = new ElementInfo(1, "H", "Hydrogen", 1.008, "[1]", "other-nonmetal", 1, 1);
        var square = new Square(null, null);
        var eventFired = false;
        square.OnChange += (_, _) => eventFired = true;

        square.SetElement(info);

        Assert.True(eventFired);
    }

    [Fact]
    public void Square_SetStatus_TriggersOnChange()
    {
        var status = new ServerInfo
        {
            Number = 1,
            Symbol = "H",
            Name = "Hydrogen",
            LastStatus = "active",
            DeviceType = 1,
            LastSeen = DateTimeOffset.UtcNow
        };
        var square = new Square(null, null);
        var eventFired = false;
        square.OnChange += (_, _) => eventFired = true;

        square.SetStatus(status);

        Assert.True(eventFired);
    }

    [Fact]
    public void Square_Name_FallsBackToElementName()
    {
        var info = new ElementInfo(1, "H", "Hydrogen", 1.008, "[1]", "other-nonmetal", 1, 1);
        var square = new Square(info, null);

        Assert.Equal("Hydrogen", square.Name);
    }

    [Fact]
    public void Square_Name_PrefersServerName()
    {
        var info = new ElementInfo(1, "H", "Hydrogen", 1.008, "[1]", "other-nonmetal", 1, 1);
        var status = new ServerInfo
        {
            Number = 1,
            Symbol = "H",
            Name = "HydrogenServer",
            LastStatus = "active",
            DeviceType = 1,
            LastSeen = DateTimeOffset.UtcNow
        };
        var square = new Square(info, status);

        Assert.Equal("HydrogenServer", square.Name);
    }

    [Fact]
    public void Square_Column_ReturnsInfoGridColumn()
    {
        var info = new ElementInfo(1, "H", "Hydrogen", 1.008, "[1]", "other-nonmetal", 1, 1);
        var square = new Square(info, null);

        Assert.Equal(1, square.Column);
    }

    [Fact]
    public void Square_Row_ReturnsInfoGridRow()
    {
        var info = new ElementInfo(1, "H", "Hydrogen", 1.008, "[1]", "other-nonmetal", 1, 1);
        var square = new Square(info, null);

        Assert.Equal(1, square.Row);
    }

    [Fact]
    public void Square_WithNoInfoOrStatus_IsNotPopulated()
    {
        var square = new Square();
        Assert.False(square.Populated);
    }

    [Fact]
    public void Square_WithOnlyStatus_IsNotPopulated()
    {
        var status = new ServerInfo { Number = 1, Symbol = "H", Name = "Hydrogen", LastStatus = "active", DeviceType = 1, LastSeen = DateTimeOffset.UtcNow };
        var square = new Square(null, status);
        Assert.False(square.Populated);
    }

    [Fact]
    public void Square_Column_WithNoInfo_ReturnsZero()
    {
        var square = new Square();
        Assert.Equal(0, square.Column);
    }

    [Fact]
    public void Square_Row_WithNoInfo_ReturnsZero()
    {
        var square = new Square();
        Assert.Equal(0, square.Row);
    }

    [Fact]
    public void Square_Name_WithNoInfoOrStatus_ReturnsUnknown()
    {
        var square = new Square();
        Assert.Equal("Unknown", square.Name);
    }

    [Fact]
    public void Square_ServerStatusClass_WithStatus_ReturnsLiveStatus()
    {
        var info = new ElementInfo(1, "H", "Hydrogen", 1.008, "[1]", "other-nonmetal", 1, 1);
        var status = new ServerInfo { Number = 1, Symbol = "H", Name = "Hydrogen", LastStatus = "active", DeviceType = 1, LastSeen = DateTimeOffset.UtcNow.AddSeconds(-30) };
        var square = new Square(info, status);

        Assert.Equal("active", square.ServerStatusClass);
    }

    [Fact]
    public void Square_SetElement_WithNull_PreservesExistingInfo()
    {
        var info = new ElementInfo(1, "H", "Hydrogen", 1.008, "[1]", "other-nonmetal", 1, 1);
        var square = new Square(info, null);

        square.SetElement(null);

        Assert.Equal(info, square.Info);
    }

    [Fact]
    public void Square_SetStatus_WithNull_PreservesExistingStatus()
    {
        var status = new ServerInfo { Number = 1, Symbol = "H", Name = "Hydrogen", LastStatus = "active", DeviceType = 1, LastSeen = DateTimeOffset.UtcNow };
        var square = new Square(null, status);

        square.SetStatus(null);

        Assert.Equal(status, square.Status);
    }

    [Fact]
    public void Square_SetLastSeen_WithExistingStatus_PreservesOtherFields()
    {
        var info = new ElementInfo(1, "H", "Hydrogen", 1.008, "[1]", "other-nonmetal", 1, 1);
        var status = new ServerInfo { Number = 1, Symbol = "H", Name = "my-server", LastStatus = "active", DeviceType = 1, LastSeen = DateTimeOffset.UtcNow.AddMinutes(-10) };
        var square = new Square(info, status);

        square.SetLastSeen(DateTimeOffset.UtcNow);

        Assert.Equal("my-server", square.Status?.Name);
    }
}
