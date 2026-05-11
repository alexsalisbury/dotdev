using DotDev.Core.Element;
using Xunit;

namespace Core.Tests;

public class ServerInfoStatusTests
{
    [Fact]
    public void LiveStatus_WhenLastSeenNull_ReturnsUntracked()
    {
        var server = new ServerInfo
        {
            Number = 1,
            Symbol = "H",
            Name = "Hydrogen",
            LastStatus = "active",
            DeviceType = 1,
            LastSeen = null
        };

        Assert.Equal("untracked", server.LiveStatus);
    }

    [Fact]
    public void LiveStatus_WhenLastStatusUntracked_ReturnsUntracked()
    {
        var server = new ServerInfo
        {
            Number = 1,
            Symbol = "H",
            Name = "Hydrogen",
            LastStatus = "untracked",
            DeviceType = 1,
            LastSeen = DateTimeOffset.UtcNow
        };

        Assert.Equal("untracked", server.LiveStatus);
    }

    [Fact]
    public void LiveStatus_WhenSeenLessThan2MinAgo_ReturnsActive()
    {
        var server = new ServerInfo
        {
            Number = 1,
            Symbol = "H",
            Name = "Hydrogen",
            LastStatus = "online",
            DeviceType = 1,
            LastSeen = DateTimeOffset.UtcNow.AddSeconds(-30)
        };

        Assert.Equal("active", server.LiveStatus);
    }

    [Fact]
    public void LiveStatus_WhenSeenBetween2And6MinAgo_ReturnsOnline()
    {
        var server = new ServerInfo
        {
            Number = 1,
            Symbol = "H",
            Name = "Hydrogen",
            LastStatus = "online",
            DeviceType = 1,
            LastSeen = DateTimeOffset.UtcNow.AddMinutes(-4)
        };

        Assert.Equal("online", server.LiveStatus);
    }

    [Fact]
    public void LiveStatus_WhenSeenBetween6And11MinAgo_ReturnsDelayed()
    {
        var server = new ServerInfo
        {
            Number = 1,
            Symbol = "H",
            Name = "Hydrogen",
            LastStatus = "online",
            DeviceType = 1,
            LastSeen = DateTimeOffset.UtcNow.AddMinutes(-8)
        };

        Assert.Equal("delayed", server.LiveStatus);
    }

    [Fact]
    public void LiveStatus_WhenSeenMoreThan11MinAgo_ReturnsOffline()
    {
        var server = new ServerInfo
        {
            Number = 1,
            Symbol = "H",
            Name = "Hydrogen",
            LastStatus = "online",
            DeviceType = 1,
            LastSeen = DateTimeOffset.UtcNow.AddMinutes(-15)
        };

        Assert.Equal("offline", server.LiveStatus);
    }

    [Fact]
    public void Generate_CreatesServerInfoWithCorrectId()
    {
        var ts = DateTimeOffset.UtcNow;
        var server = ServerInfo.Generate(42, ts);

        Assert.Equal(42, server.Number);
        Assert.Equal(ts, server.LastSeen);
        Assert.Equal("active", server.LastStatus);
    }

    [Fact]
    public void IP_ReturnsNumberAsString()
    {
        var server = new ServerInfo { Number = 42 };
        Assert.Equal("42", server.IP);
    }

    [Fact]
    public void Generate_SetsDefaultDeviceType()
    {
        var server = ServerInfo.Generate(1, null);
        Assert.Equal(404u, server.DeviceType);
    }

    [Fact]
    public void Generate_WithNullTimestamp_LiveStatusIsUntracked()
    {
        var server = ServerInfo.Generate(1, null);
        Assert.Equal("untracked", server.LiveStatus);
    }
}
