namespace HubFunctions;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public record StatusDatapoint(uint Id, DateTimeOffset Timestamp);

public static class StatusIntake
{
    [FunctionName("StatusIntake")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        [Queue("status")] IAsyncCollector<StatusDatapoint> notifications,
        ILogger log)
    {
            var result = JsonConvert.DeserializeObject<StatusDatapoint>(req.Body.ToString());
            await notifications.AddAsync(result);

        return new AcceptedResult();
    }
}
