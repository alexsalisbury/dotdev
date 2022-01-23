namespace Dotdev.Core.HexPath
{
	public interface IHoneycomb
	{
		event EventHandler<EventArgs> HoneycombChanged;
		IEnumerable<HexItem> GetItems();
		bool AddGhosts(HexLocation location);
		bool EnableGhosts(HexLocation location);
		bool AddRoot(HexLocation location);
		//bool AddItem(HexItem item);
		//void Enable(uint row, uint col);
		//void Enable(HexOrder order);
		//bool ContainsIndex(HexOrder index);
	}
}