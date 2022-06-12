namespace Dotdev.Core.HexPath;

public record AboutHex : HexItem
{
    public AboutHex(HexLocation location, bool enable) : base(location, AboutHex.DefaultStyle with {IsGhost = !enable } )
    {
    }

    public static HexStyle DefaultStyle => new HexStyle()
        { HexClass = "hexabout", Shade = HexItem.GetDefaultShade((uint)HexOrder.About), Image = HexItem.GetDefaultImage((uint)HexOrder.About) };


    public static (uint, uint) DefaultLocation => (3, 4);
}
