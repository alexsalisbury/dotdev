namespace HubFunctions;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public record StatusDatapoint(int Id, long Timestamp);

public static class StatusIntake
{
    [Function("StatusIntake")]
    [QueueOutput("elementstatus")]
    public static async Task<string> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
        FunctionContext context)
    {
        var logger = context.GetLogger("StatusIntake");

        // Read body from HttpRequestData
        var body = await new StreamReader(req.Body).ReadToEndAsync();

        // Deserialize using System.Text.Json (recommended for isolated worker)
        var data = JsonSerializer.Deserialize<StatusDatapoint>(body);

        logger.LogInformation("Received status update: {Id} at {Timestamp}",
            data?.Id, data?.Timestamp);

        // Isolated worker: return the value directly to QueueOutput
        return JsonSerializer.Serialize(data);
    }
}