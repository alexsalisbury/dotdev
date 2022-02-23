using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

// peeking a bit off of https://github.com/madebygps/signalr-functions-blazor-demo/blob/master/function/Function.cs,
// but ultimately going a different (sub)path.
namespace Dotdev.Api
{
    public record StatusDatapoint (uint Id, DateTimeOffset Timestamp);

    public static class DirectToHub
    {
        private static HttpClient httpClient = new HttpClient();
        private static string Etag = string.Empty;

        //[FunctionName("DirectToHub")]
        //public static async Task<IActionResult> Run(
        //    [HttpTrigger(AuthorizationLevel.User, "get", "post", Route = null)] HttpRequest req,
        //    ILogger log)
        //{
        //    log.LogInformation("C# HTTP trigger function processed a request.");

        //    string name = req.Query["name"];

        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    dynamic data = JsonConvert.DeserializeObject(requestBody);
        //    name = name ?? data?.name;

        //    string responseMessage = string.IsNullOrEmpty(name)
        //        ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
        //        : $"Hello, {name}. This HTTP triggered function executed successfully.";

        //    return new OkObjectResult(responseMessage);
        //}

        ////TODO: Is this for blocks?
        //[FunctionName("index")]
        //public static IActionResult GetHomePage([HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req, ExecutionContext context)
        //{
        //    var path = Path.Combine(context.FunctionAppDirectory, "content", "index.html");
        //    return new ContentResult
        //    {
        //        Content = File.ReadAllText(path),
        //        ContentType = "text/html",
        //    };
        //}

        [FunctionName("negotiate")]
        public static SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req,
            [SignalRConnectionInfo(HubName = "serverless")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }

        //[FunctionName("canary")]
        //public static async Task Canary([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer,
        //    [Queue("status")] IAsyncCollector<StatusDatapoint> notifications)
        //{
        //    var result = new StatusDatapoint(1, DateTimeOffset.UtcNow);
        //    await notifications.AddAsync(result);
        //}

        [FunctionName("intake")]
        public static async Task Intake(
            [HttpTrigger(AuthorizationLevel.User, "get", "post", Route = null)] HttpRequest req,
            [Queue("status")] IAsyncCollector<StatusDatapoint> notifications)
        {
            var result = JsonConvert.DeserializeObject<StatusDatapoint>(req.Body.ToString());
            await notifications.AddAsync(result);
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
}
