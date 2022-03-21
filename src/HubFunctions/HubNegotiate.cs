namespace Dotdev.Api;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

public static class HubNegotiateFunction
{
    [FunctionName("negotiate")]
    public static SignalRConnectionInfo Run(
        [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req,
        [SignalRConnectionInfo(HubName = "SignalRServiceHomeDotCloud")] SignalRConnectionInfo connectionInfo)
    {
        return connectionInfo;
    }
}