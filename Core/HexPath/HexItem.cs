namespace Dotdev.Core.HexPath
{
    public record HexItem (uint Index, uint Column, uint Row, string Shade, string HexClass, string Target, bool Enabled, uint[] Unlocks);
}