namespace HubFunctions;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public record StatusDatapoint(int Id, long Timestamp);

public static class StatusIntake
{
    [FunctionName("StatusIntake")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        [Queue("elementstatus")] IAsyncCollector<StatusDatapoint> notifications,
        ILogger log)
    {
        var data = JsonConvert.DeserializeObject<StatusDatapoint>(req.Body.ToString());
        await notifications.AddAsync(data);
        return new AcceptedResult();
    }
}
