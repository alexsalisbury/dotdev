namespace Dotdev.Core.Element
{
    using System;

    public class ServerInfo
    {
        public uint Number { get; set; }
        public string Symbol { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public uint DeviceType { get; set; }
        public DateTimeOffset? LastSeen { get; set; }
    }
}
