namespace Dotdev.Core.Element
{
    public class Square
    {
        public ElementInfo Info { get; set; }
        public ServerInfo Status { get; set; }
        public uint Column => Info?.Column ?? 0;
        public uint Row => Info?.Row ?? 0;
        public string Name => Status?.Name ?? Info?.Name ?? "Unknown";
        public string ServerStatusClass => Status?.Status ?? "unknown";
    }
}
