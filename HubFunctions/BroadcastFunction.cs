namespace HubFunctions;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

public class ElementBroadcastFunction
{
    [Function(nameof(BroadcastToAll))]
    [SignalROutput(HubName = "chat", ConnectionStringSetting = "SignalRConnection")]
    public static SignalRMessageAction BroadcastToAll(
      [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        using var bodyReader = new StreamReader(req.Body);
        return new SignalRMessageAction("newMessage")
        {
            Arguments = new[] { bodyReader.ReadToEnd() }
        };
    }
}