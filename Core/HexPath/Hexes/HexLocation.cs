namespace Dotdev.Core.HexPath
{
    public record HexLocation
    {
        public uint Row { get; set; }
        public uint Column { get; set; }
        public HexOrder Index { get; set; }
        public uint GridIndex => ((Row * 100) + Column);

        public HexLocation(uint row, uint column) : base()
        {
            this.Row = row;
            this.Column = column;
        }

        public HexLocation(HexOrder index, (uint, uint) coords) : this(coords.Item1, coords.Item2)
        {
            this.Index = index;
        }

        public HexLocation(HexOrder index, uint row, uint column) : this(row, column)
        {
            this.Index = index;
        }

        public HexLocation Move(uint direction, HexOrder index)
        {
            var row = direction switch
            {
                1 => this.Row - 1,
                2 => this.Row - 1,
                3 => this.Row,
                4 => this.Row + 1,
                5 => this.Row,
                _ => this.Row + 1,
            };

            var col = direction switch
            {
                1 => this.Column,
                2 => this.Column + 1,
                3 => this.Column + 1,
                4 => this.Column,
                _ => this.Column - 1,
            };

            return this with { Index = index,  Row = row, Column = col};
        }
    }
}