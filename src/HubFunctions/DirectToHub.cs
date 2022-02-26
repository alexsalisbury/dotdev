namespace Dotdev.Api;

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public static class DirectToHubFunction
{
    [FunctionName("DirectToHub")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        string name = req.Query["name"];

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        name = name ?? data?.name;

        string responseMessage = string.IsNullOrEmpty(name)
            ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            : $"Hello, {name}. This HTTP triggered function executed successfully.";

        return new OkObjectResult(responseMessage);
    }
    //[FunctionName("broadcast")]
    //public static async Task QueueToBroadcast(
    //   [QueueTrigger("status")] StatusDatapoint data,
    //    [SignalR(HubName = "serverless")] IAsyncCollector<SignalRMessage> signalRMessages)
    //{
    //    await signalRMessages.AddAsync(
    //        new SignalRMessage
    //        {
    //            Target = "status",
    //            Arguments = new object[] { data.Id, data.Timestamp }
    //        });
    //}
}