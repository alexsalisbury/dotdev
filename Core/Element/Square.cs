namespace Dotdev.Core.Element
{
    public class Square
    {

        public event EventHandler OnChange;

        public Square() { }
        public Square(ElementInfo? info, ServerInfo? status)
        {
            Info = info;
            Status = status;
        }

        public void SetElement(ElementInfo? info)
        {
            this.Info  = info ?? this.Info;
            this.OnChange?.Invoke(this, new EventArgs());
        }

        public void SetStatus(ServerInfo? status)
        {
            this.Status = status ?? this.Status;
            this.OnChange?.Invoke(this, new EventArgs());
        }

        public void SetLastSeen(DateTimeOffset timestamp)
        {
            this.Status = this.Status switch
            {
                null => ServerInfo.Generate(this.Info.Number, timestamp),
                _ => this.Status with { LastSeen = timestamp }
            };
            this.OnChange?.Invoke(this, new EventArgs());
        }

        public ElementInfo? Info { get; private set; }
        public ServerInfo? Status { get; private set; }
        public bool Populated => ((Info is not null) && (Status is not null));
        public int Column => Info?.GridColumn ?? 0;
        public int Row => Info?.GridRow ?? 0;
        public string Name => Status?.Name ?? Info?.ElementName ?? "Unknown";
        public string ServerStatusClass => Status?.LiveStatus ?? "unknown";
    }
}
