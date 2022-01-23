namespace Dotdev.Core.HexPath
{
    public record BlankItem : HexItem
    {
        public BlankItem(HexLocation location, HexStyle style) : base(location, style)
        {
            Location = new HexLocation(HexOrder.Blank, location.Row, location.Column);
            Style = style;
        }
    }
}