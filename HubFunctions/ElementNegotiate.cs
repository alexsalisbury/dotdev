namespace HubFunctions;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;

public static class HubNegotiateFunction
{
    [Function("negotiate")]
    public static SignalRConnectionInfo Run(
<<<<<<< cleanup
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
=======
        [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req,
>>>>>>> development
        [SignalRConnectionInfoInput(HubName = "dotdev")] SignalRConnectionInfo connectionInfo)
    {
        return connectionInfo;
    }
}