namespace HubFunctions;

using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

public class CanaryFunction
{
    private readonly QueueClient _queueClient;
    private readonly ILogger _logger;

    public CanaryFunction(QueueClient queueClient, ILoggerFactory loggerFactory)
    {
        _queueClient = queueClient;
        _logger = loggerFactory.CreateLogger<CanaryFunction>();
    }

    /// <summary>
    /// This canary skips past the IoT workflow to help us verify the hub/display end of the pipeline.
    /// </summary>
    /// <remarks>Using Copernicium for Canary; Ca (Calcium) was a previous machine name.</remarks>
    [Function("Canary")]
    public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
    {
        var cn = new StatusDatapoint(112, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        var message = JsonSerializer.Serialize(cn);

        await _queueClient.SendMessageAsync(message);
        _logger.LogInformation("Canary sent for Copernicium at {Time}", DateTimeOffset.UtcNow);
    }
}
