//namespace Dotdev.Client.Hubs;

//using Microsoft.AspNetCore.SignalR;

//public class StatusHub : Hub
//{
//    public const string HubUrl = "/elementstatus";

//    public void BroadcastStatusPacket(int id, DateTimeOffset timestamp)
//    {
//        Clients.All.SendAsync("elementstatus", new[] { id, timestamp.ToUnixTimeSeconds() });
//    }

//    public override Task OnConnectedAsync()
//    {
//        Console.WriteLine($"{Context.ConnectionId} connected");
//        return base.OnConnectedAsync();
//    }

//    public override async Task OnDisconnectedAsync(Exception e)
//    {
//        Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
//        await base.OnDisconnectedAsync(e);
//    }
//}