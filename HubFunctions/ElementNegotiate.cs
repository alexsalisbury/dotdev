namespace HubFunctions;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;

public static class HubNegotiateFunction
{
    [Function("negotiate")]
    public static SignalRConnectionInfo Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
        [SignalRConnectionInfoInput(HubName = "dotdev")] SignalRConnectionInfo connectionInfo)
    {
        return connectionInfo;
    }
}