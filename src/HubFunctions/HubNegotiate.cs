namespace Dotdev.Api;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

public static class HubNegotiateFunction
{
    [FunctionName("HubNegotiate")]
    public static SignalRConnectionInfo Run(
        [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req,
        [SignalRConnectionInfo(HubName = "homedotcloud")] SignalRConnectionInfo connectionInfo)
    {
        return connectionInfo;
    }
}