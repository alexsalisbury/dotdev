namespace Dotdev.Core.HexPath
{
    public record HexStyle
    {
        private const double piOver3 = Math.PI / 3;
        private const double piOver6 = Math.PI / 6;
        private const uint size = 100;

        public string? Shade { get; set; }
        public string? HexClass { get; set; }
        public string? Target { get; set; }
        public bool IsGhost { get; set; }
        public string Size => size.ToString();
        public string Points => $"{GetPair(0)} {GetPair(1)} {GetPair(2)} {GetPair(3)} {GetPair(4)} {GetPair(5)} {GetPair(6)}";
        public string MaxSizes => $"max-height:{size}px;max-width:{size}px;";
        public string ViewBox => $"0 0 {size} {size}";

        // Rounding to only one digit past the point is a bit aggressive, but it's actually pretty okay for the standard size (const'd above) of 100. 
        private string GetPair(int idx) => $"{Math.Round(size / 2 * (1 + Math.Cos((idx * piOver3) - piOver6)), 1)},{Math.Round(size / 2 * (1 + Math.Sin((idx * piOver3) - piOver6)), 1)}";
    }
}