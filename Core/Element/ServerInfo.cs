namespace Dotdev.Core.Element
{
    using System;

    public record ServerInfo
    {
        public int Number { get; set; }
        public string Symbol { get; set; }
        public string LastStatus { get; set; }
        public string Name { get; set; }
        public string IP => Number.ToString();
        public uint DeviceType { get; set; }
        public DateTimeOffset? LastSeen { get; set; }
        public string LiveStatus => GetCurrentStatus(LastStatus, LastSeen);

        public static ServerInfo Generate(int id, DateTimeOffset? ts)
        {
            return new ServerInfo()
            {
                DeviceType = 404,
                LastSeen = ts,
                LastStatus = "active",
                Name = "",
                Number = id,
                Symbol = ""
            };
        }

        private static string GetCurrentStatus(string lastStatus, DateTimeOffset? lastSeen)
        {
            if (lastSeen == null)
            {
                return "untracked";
            }

            return lastStatus switch
            {
                "untracked" => "untracked",
                _ => GetStatusFromTime(DateTimeOffset.UtcNow, lastSeen)
            };
        }

        private static string GetStatusFromTime(DateTimeOffset checkTime, DateTimeOffset? lastSeen)
        {
            var delta = checkTime - lastSeen;
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
