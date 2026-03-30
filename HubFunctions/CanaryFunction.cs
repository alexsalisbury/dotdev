using HubFunctions;
using Microsoft.Azure.Functions.Worker;
using System.Text.Json;

public static class CanaryFunction
{
    [Function("Canary")]
    [QueueOutput("elementstatus")]
    public static string Run(
        [TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo,
        FunctionContext context)
    {
        var logger = context.GetLogger("Canary");

        var datapoint = new StatusDatapoint(112, DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        logger.LogInformation("Enqueuing Canary: {Id} at {Timestamp}", datapoint.Id, datapoint.Timestamp);

        return JsonSerializer.Serialize(datapoint);
    }
}