using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

public static class HubNegotiateFunction
{
    [Function("negotiate")]
    public static string Run(
        [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequestData req,
        [SignalRConnectionInfoInput(HubName = "dotdev")] string connectionInfo)
    {
        // The connectionInfo *string* is already JSON and ready to return.
        return connectionInfo;
    }
}