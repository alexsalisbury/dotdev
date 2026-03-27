namespace HubFunctions;

using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Azure.WebJobs;

public static class CanaryFunction
{
    /// <summary>
    /// This canary skips past the IoT workflow to help us verify the hub/display end of the pipeline.
    /// </summary>
    /// <param name="myTimer"></param>
    /// <param name="queueClient">Represents a Queue in Azure</param>
    /// <remarks>Using Copernicium for Canary; Ca (Calcium) was a previous machine name.</remarks>
    /// <returns>Task</returns>
    [FunctionName("Canary")]
    public static async Task Run(
        [TimerTrigger("0 */5 * * * *")] TimerInfo myTimer,
        [Queue("elementstatus")] QueueClient queueClient)
    {
        var cn = new StatusDatapoint(112, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        await queueClient.SendMessageAsync(JsonSerializer.Serialize(cn));

    }
}