namespace HubFunctions;

using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

public class ElementBroadcastFunction
{
    private readonly ILogger _logger;

    public ElementBroadcastFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ElementBroadcastFunction>();
    }

    [Function("broadcast")]
    [SignalROutput(HubName = "dotdev")]
    public SignalRMessageAction Run(
        [QueueTrigger("elementstatus")] StatusDatapoint data,
        FunctionContext context)
    {
        _logger.LogInformation("Broadcasting element {Id}", data.Id);

        return new SignalRMessageAction("elementstatus", new object[] { data.Id, data.Timestamp });
    }
}
