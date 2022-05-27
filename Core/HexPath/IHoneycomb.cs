namespace Dotdev.Core.HexPath
{
	public interface IHoneycomb
	{
		event EventHandler<EventArgs> HoneycombChanged;
		IEnumerable<HexItem> GetItems();
		ValueTask<bool> AddGhosts(HexLocation location);
		ValueTask<bool> EnableGhosts(HexLocation location);
		bool AddRoot(HexLocation location);

		Task UnlockAsync();
		//bool AddItem(HexItem item);
		//void Enable(uint row, uint col);
		//void Enable(HexOrder order);
		//bool ContainsIndex(HexOrder index);
	}
}