namespace Dotdev.Core.Element
{
    public class Square
    {
        public void SetLastSeen(DateTimeOffset timestamp) => this.Status = this.Status == null ? null : this.Status with { LastSeen = timestamp }; 
        public ElementInfo Info { get; set; }
        public ServerInfo? Status { get; set; }
        public uint Column => Info?.GridColumn ?? 0;
        public uint Row => Info?.GridRow ?? 0;
        public string Name => Status?.Name ?? Info?.ElementName ?? "Unknown";
        public string ServerStatusClass => Status?.Status ?? "unknown";
    }
}
