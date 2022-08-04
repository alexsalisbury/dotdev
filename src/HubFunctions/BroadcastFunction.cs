namespace HubFunctions;

using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Data.SqlClient;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public static class ElementBroadcastFunction
{
    [FunctionName("broadcast")]
    public static async Task QueueToBroadcast(
    [QueueTrigger("elementstatus")] StatusDatapoint data,
    [SignalR(HubName = "dotdev")] IAsyncCollector<SignalRMessage> signalRMessages, 
    ILogger log,
    ExecutionContext context)
    {
        await signalRMessages.AddAsync(
            new SignalRMessage
            {
                Target = "elementstatus",
                Arguments = new object[] { data.Id, data.Timestamp }
            });
    }
}