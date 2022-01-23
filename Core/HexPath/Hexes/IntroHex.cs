namespace Dotdev.Core.HexPath
{
    public record IntroHex : HexItem
    {
        public IntroHex(HexLocation location) : base(location, DefaultStyle)
            //: base(HexOrder.Intro, column, row, "#A1AEFF", "hexOne", "./hex_placeholder.png", true, (5, HexOrder.About))
        {
            this.Location = location;
            this.Style = IntroHex.DefaultStyle;
            this.Unlocks = new[] {((uint)3, HexOrder.About) };
        }

        public static HexStyle DefaultStyle => new HexStyle()
                { HexClass = "hexroot", Shade = HexItem.GetDefaultShade((uint)HexOrder.Intro), Target = HexItem.GetDefaultTarget((uint)HexOrder.Intro), IsGhost = false };


        public static (uint, uint) DefaultLocation => (2,5);
    }
}