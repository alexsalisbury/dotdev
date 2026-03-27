namespace DotDev.Core.Element;

using System.Collections.Generic;

public class ElementInfo
{
    public int Number { get; set; }
    public string Symbol { get; set; }
    public string ElementName { get; set; }
    public string Mass { get; set; }
    public string Weight { get; set; }
    public string Material { get; set; }
    public int GridColumn { get; set; }
    public int GridRow { get; set; }

    public ElementInfo() { }

    public ElementInfo(int number, string symbol, string name, string mass, string material, int column, int row)
    {
        this.Number = number;
        this.Symbol = symbol;
        this.ElementName = name;
        this.Mass = mass;
        this.Material = material;
        this.GridColumn = column;
        this.GridRow = row;
    }

    public ElementInfo(int number, string symbol, string name, double mass, string weightstr, string material, int column, int row) :
        this(number, symbol, name, mass.ToString(), weightstr, material, column, row)
    {
    }

    public ElementInfo(int number, string symbol, string name, string mass, string weightstr, string material, int column, int row)
    {
        this.Number = number;
        this.Symbol = symbol;
        this.ElementName = name;
        this.Mass = mass;
        this.Weight = weightstr;
        this.Material = material;
        this.GridColumn = column;
        this.GridRow = row;
    }
}
