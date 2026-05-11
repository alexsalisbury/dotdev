using Bunit;
using DotDev.Client.Shared.Status;
using DotDev.Core.Element;
using Xunit;

namespace Core.Tests;

public class ElementComponentTests : TestContext
{
    private static ElementInfo MakeElement(
        int number = 1,
        string symbol = "H",
        string name = "Hydrogen",
        string material = "other-nonmetal",
        int column = 1,
        int row = 1) =>
        new ElementInfo(number, symbol, name, "1.008", "[1]", material, column, row);

    [Fact]
    public void Element_RendersAtomicNumber()
    {
        var info = MakeElement(number: 6);
        var cut = Render<Element>(p => p.Add(c => c.Info, info).Add(c => c.StatusKey, "active"));
        Assert.Equal("6", cut.Find(".atomic-number").TextContent);
    }

    [Fact]
    public void Element_RendersSymbol()
    {
        var info = MakeElement(symbol: "Au");
        var cut = Render<Element>(p => p.Add(c => c.Info, info).Add(c => c.StatusKey, "online"));
        Assert.Contains("Au", cut.Find(".symbol").TextContent);
    }

    [Fact]
    public void Element_RendersName()
    {
        var info = MakeElement(name: "Carbon");
        var cut = Render<Element>(p => p.Add(c => c.Info, info).Add(c => c.StatusKey, "active"));
        Assert.Contains("Carbon", cut.Find(".name").TextContent);
    }

    [Fact]
    public void Element_RendersStatusKey()
    {
        var info = MakeElement();
        var cut = Render<Element>(p => p.Add(c => c.Info, info).Add(c => c.StatusKey, "offline"));
        Assert.Contains("offline", cut.Find(".status").TextContent);
    }

    [Fact]
    public void Element_ElementClass_IncludesMaterialAndStatusKey()
    {
        var info = MakeElement(material: "noble-gas");
        var cut = Render<Element>(p => p.Add(c => c.Info, info).Add(c => c.StatusKey, "active"));
        var divClass = cut.Find("div").ClassName ?? string.Empty;
        Assert.Contains("noble-gas", divClass);
        Assert.Contains("active", divClass);
    }

    [Fact]
    public void Element_ElementClass_IncludesElementPrefix()
    {
        var info = MakeElement(material: "metalloid");
        var cut = Render<Element>(p => p.Add(c => c.Info, info).Add(c => c.StatusKey, "online"));
        var divClass = cut.Find("div").ClassName ?? string.Empty;
        Assert.Contains("element", divClass);
        Assert.Contains("metalloid", divClass);
    }

    [Fact]
    public void Element_Number_ReturnsFromInfo()
    {
        var info = MakeElement(number: 79);
        var cut = Render<Element>(p => p.Add(c => c.Info, info).Add(c => c.StatusKey, "active"));
        Assert.Equal(79, cut.Instance.Number);
    }

    [Fact]
    public void Element_Column_ReturnsFromInfo()
    {
        var info = MakeElement(column: 11);
        var cut = Render<Element>(p => p.Add(c => c.Info, info).Add(c => c.StatusKey, "active"));
        Assert.Equal(11, cut.Instance.Column);
    }

    [Fact]
    public void Element_Row_ReturnsFromInfo()
    {
        var info = MakeElement(row: 6);
        var cut = Render<Element>(p => p.Add(c => c.Info, info).Add(c => c.StatusKey, "active"));
        Assert.Equal(6, cut.Instance.Row);
    }

    [Fact]
    public void Element_Symbol_ReturnsFromInfo()
    {
        var info = MakeElement(symbol: "Fe");
        var cut = Render<Element>(p => p.Add(c => c.Info, info).Add(c => c.StatusKey, "active"));
        Assert.Equal("Fe", cut.Instance.Symbol);
    }

    [Fact]
    public void Element_Name_ReturnsFromInfo()
    {
        var info = MakeElement(name: "Iron");
        var cut = Render<Element>(p => p.Add(c => c.Info, info).Add(c => c.StatusKey, "active"));
        Assert.Equal("Iron", cut.Instance.Name);
    }

    [Fact]
    public void Element_Material_ReturnsFromInfo()
    {
        var info = MakeElement(material: "transition-metal");
        var cut = Render<Element>(p => p.Add(c => c.Info, info).Add(c => c.StatusKey, "active"));
        Assert.Equal("transition-metal", cut.Instance.Material);
    }

    [Fact]
    public void Element_ElementBasic_CombinesElementAndMaterial()
    {
        var info = MakeElement(material: "alkali-metal");
        var cut = Render<Element>(p => p.Add(c => c.Info, info).Add(c => c.StatusKey, "active"));
        Assert.Equal("element alkali-metal", cut.Instance.ElementBasic);
    }

    [Fact]
    public void Element_ElementClass_CombinesBasicAndStatusKey()
    {
        var info = MakeElement(material: "noble-gas");
        var cut = Render<Element>(p => p.Add(c => c.Info, info).Add(c => c.StatusKey, "online"));
        Assert.Equal("element noble-gas online", cut.Instance.ElementClass);
    }

    [Fact]
    public void Element_Material_ReturnsEmptyStringWhenInfoIsNull()
    {
        var info = MakeElement();
        var cut = Render<Element>(p => p.Add(c => c.Info, info).Add(c => c.StatusKey, "active"));
        Assert.NotNull(cut.Instance.Material);
    }
}
