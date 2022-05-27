namespace Dotdev.Core.HexPath;

public record StatusHex : HexItem
{
    private List<HexItem> result;

    public StatusHex(HexLocation location, bool enable) : base(location, StatusHex.DefaultStyle with { IsGhost = !enable, Target = "/Status" })
    {
        
    }

    public static HexStyle DefaultStyle => new HexStyle()
    { HexClass = "hexstatus", Shade = HexItem.GetDefaultShade((uint)HexOrder.Status), Image = HexItem.GetDefaultImage((uint)HexOrder.Status) };


    public static (uint, uint) DefaultLocation => (3, 5);
}
