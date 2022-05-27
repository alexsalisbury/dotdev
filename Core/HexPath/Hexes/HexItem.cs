namespace Dotdev.Core.HexPath
{
    public abstract record HexItem
    {

        public HexLocation Location { get; init; }
        public HexStyle Style { get; init; }
        public (uint, HexOrder)[] Unlocks { get; init; }

        public uint GridIndex => Location?.GridIndex ?? 0;
        public bool IsGhost => Style?.IsGhost ?? false;

        internal static (uint, HexOrder)[] Empty { get; set; }

        static HexItem()
        {
            Empty = Empty ?? Array.Empty<(uint, HexOrder)>();
        }

        protected HexItem(HexLocation location, HexStyle style)
        {
            this.Location = location;
            this.Style = style;
            this.Unlocks = Empty;
        }

        protected HexItem(HexLocation location, HexStyle style, params (uint, HexOrder)[] unlocks)
        {
            this.Location = location;
            this.Style = style;
            this.Unlocks = unlocks ?? Empty;
        }

        internal (uint, uint) GetCoordinates()
        {
            return (Location?.Row ?? 0, Location?.Column ?? 0);
        }

        protected internal static string GetDefaultTarget(uint idx) => $"./hex_placeholder_{idx}.png";

        protected internal static string GetDefaultShade(uint idx)
        {
            return idx switch
            {
                0 => "transparent",
                1 => "#E9967A",
                2 => "#800080",
                3 => "#DB7093",
                _ => "#FFE4E1"
            };
        }

        internal IEnumerable<HexItem> GetGhosts(bool enable)
        {
            foreach (var pair in this.Unlocks)
            {
                var next = this.Location.Move(pair.Item1, pair.Item2);

                HexItem? item = pair.Item2 switch 
                {
                    //HexOrder.Blank => new BlankItem(),
                    HexOrder.About => new AboutHex(next, enable),
                    HexOrder.Status => new StatusHex(next, enable),
                    _ => null
                };

                if (item != null)
                {
                    yield return item;
                }
            }
        }
    }
}