namespace Dotdev.Core.HexPath
{
    public record IntroHex : HexItem
    {
        public IntroHex(HexLocation location) : base(location, DefaultStyle, DefaultText)
            //: base(HexOrder.Intro, column, row, "#A1AEFF", "hexOne", "./hex_placeholder.png", true, (5, HexOrder.About))
        {
            this.Location = location;
            this.Style = IntroHex.DefaultStyle;
            this.Unlocks = new[] {((uint)3, HexOrder.About), ((uint)4, HexOrder.Status) };
        }

        public static HexStyle DefaultStyle => new HexStyle()
                { HexClass = "hexroot", Shade = HexItem.GetDefaultShade((uint)HexOrder.Intro), Image = HexItem.GetDefaultImage((uint)HexOrder.Intro), IsGhost = false };


        public static (uint, uint) DefaultLocation => (2,5);

        public static string[] DefaultText = { "Welcome...initializing...", "1/3 modules loaded", "[This space intentionally left minimalist]", ">" };
    }
}