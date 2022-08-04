namespace Dotdev.Core.HexPath;

public record AboutHex : HexItem
{
    public AboutHex(HexLocation location, bool enable) : base(location, AboutHex.DefaultStyle with { IsGhost = !enable }, AboutHex.DefaultText)
    {
    }

    public static HexStyle DefaultStyle => new HexStyle()
    { HexClass = "hexabout", Shade = HexItem.GetDefaultShade((uint)HexOrder.About), Image = HexItem.GetDefaultImage((uint)HexOrder.About) };


    public static (uint, uint) DefaultLocation => (3, 4);
    public static string[] DefaultText = {"About this site:", "This site is a playground for personal learning.", "Please enjoy responsibly.", ">" };
}
