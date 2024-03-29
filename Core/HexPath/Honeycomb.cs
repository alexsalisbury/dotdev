﻿namespace Dotdev.Core.HexPath
{
    public class Honeycomb : IHoneycomb
    {
        private Dictionary<uint, HexItem> hexMap = new();
        private static bool globalUnlock = false;
        private HexItem root;

        public event EventHandler<EventArgs> HoneycombChanged;

        //public static Ghost GetGhost(HexLocation location, HexStyle style)
        //{
        //    bool unlock = globalUnlock || location.Index == HexOrder.Intro;
        //    style.HexClass = unlock ? "hex" : "hexghost";
        //    return new Ghost(location, style);
        //}


        public ValueTask<bool> AddGhosts(HexLocation location) => WalkGhosts(location, false);
        public ValueTask<bool> EnableGhosts(HexLocation location) => WalkGhosts(location, true);

        public async Task UnlockAsync() => WalkGhosts(this.root.Location, true, true);

        public IEnumerable<HexItem> GetItems() => hexMap.Values.ToArray();

        public bool AddItem(HexItem item)
        {
            // we'll never really have too many hexes here unless something goes horribly wrong.
            if (!hexMap.ContainsKey(item.GridIndex))
            {
                // Stamp if nothing there yet. Good job!
                hexMap[item.GridIndex] = item;
                HoneycombChanged?.Invoke(this, new EventArgs());
                return true;
            }

            if (item.IsGhost)
            {
                // Ghosts can't replace existing items (not even other ghosts.)
                return false;
            }

            var existing = hexMap[item.GridIndex];
            if (existing.IsGhost)
            {
                // Non-ghosts replace ghosts...
                hexMap[item.GridIndex] = item;
                HoneycombChanged?.Invoke(this, new EventArgs());
                return true;
            }

            return false;
        }

        public bool AddRoot(HexLocation location)
        {
            root = new IntroHex(location);
            return this.AddItem(root);
        }

        private async ValueTask<bool> WalkGhosts(HexLocation location, bool enable, bool recurse = false)
        {
            if (recurse)
            {
                await Task.Delay(1000);
            }

            bool success = true;
            var source = hexMap[location.GridIndex];

            foreach (var s in source?.GetGhosts(enable) ?? new List<HexItem>())
            {
                success &= this.AddItem(s);
                if (recurse)
                {
                    await this.WalkGhosts(s.Location, enable, recurse);
                }
            }

            return success;
        }
    }
}