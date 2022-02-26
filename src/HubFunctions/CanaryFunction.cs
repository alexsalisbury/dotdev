namespace HubFunctions;

using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

public static class CanaryFunction
{
    [FunctionName("Canary")]
    public static async Task Run(
        [TimerTrigger("0 */5 * * * *")] TimerInfo myTimer,
        [Queue("status")] IAsyncCollector<StatusDatapoint> notifications)
    {
        var result = new StatusDatapoint(1, DateTimeOffset.UtcNow);
        await notifications.AddAsync(result);
    }
}