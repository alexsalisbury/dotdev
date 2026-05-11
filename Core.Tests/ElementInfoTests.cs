using DotDev.Core.Element;
using Xunit;

namespace Core.Tests;

public class ElementInfoTests
{
    [Fact]
    public void ElementInfo_DefaultConstructor_CreatesInstance()
    {
        var info = new ElementInfo();
        Assert.NotNull(info);
    }

    [Fact]
    public void ElementInfo_DefaultConstructor_HasDefaultValues()
    {
        var info = new ElementInfo();
        Assert.Equal(0, info.Number);
        Assert.Equal(0, info.GridColumn);
        Assert.Equal(0, info.GridRow);
    }

    [Fact]
    public void ElementInfo_SevenParamConstructor_SetsProperties()
    {
        var info = new ElementInfo(6, "C", "Carbon", "12.011", "organic", 14, 2);
        Assert.Equal(6, info.Number);
        Assert.Equal("C", info.Symbol);
        Assert.Equal("Carbon", info.ElementName);
        Assert.Equal("12.011", info.Mass);
        Assert.Equal("organic", info.Material);
        Assert.Equal(14, info.GridColumn);
        Assert.Equal(2, info.GridRow);
    }

    [Fact]
    public void ElementInfo_SevenParamConstructor_WeightIsNull()
    {
        // 7-param ctor doesn't set Weight
        var info = new ElementInfo(6, "C", "Carbon", "12.011", "organic", 14, 2);
        Assert.Null(info.Weight);
    }

    [Fact]
    public void ElementInfo_EightParamStringConstructor_SetsWeight()
    {
        var info = new ElementInfo(6, "C", "Carbon", "12.011", "[2,4]", "organic", 14, 2);
        Assert.Equal("[2,4]", info.Weight);
    }

    [Fact]
    public void ElementInfo_EightParamStringConstructor_SetsAllProperties()
    {
        var info = new ElementInfo(79, "Au", "Gold", "196.97", "[2,8,18,32,18,1]", "transition-metal", 11, 6);
        Assert.Equal(79, info.Number);
        Assert.Equal("Au", info.Symbol);
        Assert.Equal("Gold", info.ElementName);
        Assert.Equal("196.97", info.Mass);
        Assert.Equal("[2,8,18,32,18,1]", info.Weight);
        Assert.Equal("transition-metal", info.Material);
        Assert.Equal(11, info.GridColumn);
        Assert.Equal(6, info.GridRow);
    }

    [Fact]
    public void ElementInfo_DoubleMassConstructor_ConvertsMassToString()
    {
        var info = new ElementInfo(1, "H", "Hydrogen", 1.008, "[1]", "other-nonmetal", 1, 1);
        Assert.Equal("1.008", info.Mass);
    }

    [Fact]
    public void ElementInfo_DoubleMassConstructor_SetsWeight()
    {
        var info = new ElementInfo(1, "H", "Hydrogen", 1.008, "[1]", "other-nonmetal", 1, 1);
        Assert.Equal("[1]", info.Weight);
    }

    [Fact]
    public void ElementInfo_Properties_AreSettable()
    {
        var info = new ElementInfo();
        info.Number = 2;
        info.Symbol = "He";
        info.ElementName = "Helium";
        info.Mass = "4.0026";
        info.Weight = "[2]";
        info.Material = "noble-gas";
        info.GridColumn = 18;
        info.GridRow = 1;

        Assert.Equal(2, info.Number);
        Assert.Equal("He", info.Symbol);
        Assert.Equal("Helium", info.ElementName);
        Assert.Equal("4.0026", info.Mass);
        Assert.Equal("[2]", info.Weight);
        Assert.Equal("noble-gas", info.Material);
        Assert.Equal(18, info.GridColumn);
        Assert.Equal(1, info.GridRow);
    }
}
