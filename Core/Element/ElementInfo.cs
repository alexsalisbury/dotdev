namespace Dotdev.Core.Element
{
    using System.Collections.Generic;

    public class ElementInfo
    {
        public uint Number { get; set; }
        public string Symbol { get; set; }
        public string ElementName { get; set; }
        public string Mass { get; set; }
        public IEnumerable<uint> Weights { get; set; }
        public string Weight { get; set; }
        public string Material { get; set; }
        public uint GridColumn { get; set; }
        public uint GridRow { get; set; }

        public ElementInfo() { }

        public ElementInfo(uint number, string symbol, string name, double mass, IEnumerable<uint> weight, string material, uint column, uint row) :
            this(number, symbol, name, mass.ToString(), weight, material, column, row)
        {
        }

        public ElementInfo(uint number, string symbol, string name, string mass, IEnumerable<uint> weight, string material, uint column, uint row)
        {
            this.Number = number;
            this.Symbol = symbol;
            this.ElementName = name;
            this.Mass = mass;
            this.Weights = weight;
            this.Weight = "[" + string.Join(',', weight) + "]";
            this.Material = material;
            this.GridColumn = column;
            this.GridRow = row;
        }

        public ElementInfo(uint number, string symbol, string name, double mass, string weightstr, string material, uint column, uint row) :
            this(number, symbol, name, mass.ToString(), weightstr, material, column, row)
        {
        }

        public ElementInfo(uint number, string symbol, string name, string mass, string weightstr, string material, uint column, uint row)
        {
            this.Number = number;
            this.Symbol = symbol;
            this.ElementName = name;
            this.Mass = mass;
            this.Weight = weightstr;
            this.Material = material;
            this.GridColumn = column;
            this.GridRow = row;

            var result = new List<uint>();

            foreach (var s in weightstr.TrimStart('[').TrimEnd(']').Split(','))
            {
                if (uint.TryParse(s, out uint count))
                {
                    result.Add(count);
                }
            }

            this.Weights = result;
        }
    }
}
