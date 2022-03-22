namespace Dotdev.Core.Element
{
    using System;

    public record ServerInfo
    {
        public int Number { get; set; }
        public string Symbol { get; set; }
        public string LastStatus { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public uint DeviceType { get; set; }
        public DateTimeOffset? LastSeen { get; set; }
        public string LiveStatus => GetCurrentStatus();

        private string GetCurrentStatus()
        {
            return LastStatus switch
            {
              //  "online" => "online",
               // "delayed" => "delayed",
               // "offline" => "offline",
                "untracked" => "untracked",
                _ => GetStatusFromTime(DateTimeOffset.UtcNow)
            };
        }

        private string GetStatusFromTime(DateTimeOffset ts)
        {
            var delta = ts - LastSeen;
            return delta?.TotalMinutes switch
            {
                null => "unknown",
                < 2 => "active",
                < 6 => "online",
                < 11 => "delayed",
                > 11 => "offline",
                _ => "unknown"
            };
        }
    }
}
