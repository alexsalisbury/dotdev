namespace Dotdev.Core.Element
{
    using System.Collections.Generic;

    public class ElementInfo
    {
        public uint Number { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Mass { get; set; }
        public IEnumerable<uint> Weight { get; set; }
        public string WeightStr { get; set; }
        public string Material { get; set; }
        public uint Column { get; set; }
        public uint Row { get; set; }

        public ElementInfo(uint number, string symbol, string name, double mass, IEnumerable<uint> weight, string material, uint column, uint row) :
            this(number, symbol, name, mass.ToString(), weight, material, column, row)
        {
        }

        public ElementInfo(uint number, string symbol, string name, string mass, IEnumerable<uint> weight, string material, uint column, uint row)
        {
            this.Number = number;
            this.Symbol = symbol;
            this.Name = name;
            this.Mass = mass;
            this.Weight = weight;
            this.WeightStr = "[" + string.Join(',', weight) + "]";
            this.Material = material;
            this.Column = column;
            this.Row = row;
        }

        public ElementInfo(uint number, string symbol, string name, double mass, string weightstr, string material, uint column, uint row) :
            this(number, symbol, name, mass.ToString(), weightstr, material, column, row)
        {
        }

        public ElementInfo(uint number, string symbol, string name, string mass, string weightstr, string material, uint column, uint row)
        {
            this.Number = number;
            this.Symbol = symbol;
            this.Name = name;
            this.Mass = mass;
            this.WeightStr = weightstr;
            this.Material = material;
            this.Column = column;
            this.Row = row;

            var result = new List<uint>();

            foreach (var s in weightstr.TrimStart('[').TrimEnd(']').Split(','))
            {
                if (uint.TryParse(s, out uint count))
                {
                    result.Add(count);
                }
            }

            this.Weight = result;
        }
    }
}
