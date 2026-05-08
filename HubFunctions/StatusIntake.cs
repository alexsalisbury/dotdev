namespace HubFunctions;

using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.IO;
using System.Threading.Tasks;

public record StatusDatapoint(int Id, long Timestamp);

public class StatusIntake
{
    private readonly QueueClient _queueClient;

    public StatusIntake(QueueClient queueClient)
    {
        _queueClient = queueClient;
    }

    [Function("StatusIntake")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        var body = await new StreamReader(req.Body).ReadToEndAsync();

        await _queueClient.SendMessageAsync(body);

        var response = req.CreateResponse(System.Net.HttpStatusCode.Accepted);
        return response;
    }
}