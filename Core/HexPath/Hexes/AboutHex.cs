namespace Dotdev.Core.HexPath
{
    public record AboutHex : HexItem
    {
        private List<HexItem> result;

        public AboutHex(HexLocation location, bool enable) : base(location, AboutHex.DefaultStyle with {IsGhost = !enable } )
        {
        }

        public static HexStyle DefaultStyle => new HexStyle()
        { HexClass = "hexabout", Shade = HexItem.GetDefaultShade((uint)HexOrder.About), Target = HexItem.GetDefaultTarget((uint)HexOrder.About) };


        public static (uint, uint) DefaultLocation => (3, 5);
    }
}