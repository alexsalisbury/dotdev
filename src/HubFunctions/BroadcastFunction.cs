namespace HubFunctions;

using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Data.SqlClient;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public static class ElementBroadcastFunction
{
    [FunctionName("broadcast")]
    public static async Task QueueToBroadcast(
    [QueueTrigger("elementstatus")] StatusDatapoint data,
    [SignalR(HubName = "dotdev")] IAsyncCollector<SignalRMessage> signalRMessages, 
    ILogger log,
    ExecutionContext context)
    {
        await signalRMessages.AddAsync(
            new SignalRMessage
            {
                Target = "elementstatus",
                Arguments = new object[] { data.Id, data.Timestamp }
            });

        await UpdateDatabase(data, log, context);
    }
    public static async Task UpdateDatabase(StatusDatapoint data, ILogger log, ExecutionContext context)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(context.FunctionAppDirectory)
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var str = config.GetConnectionString("dotdev");
        using (SqlConnection conn = new SqlConnection(str))
        {
            conn.Open();
            var text = "dd_ServerUpdate";

            using (SqlCommand cmd = new SqlCommand(text, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Number", SqlDbType.Int);
                cmd.Parameters["@Number"].Value = data.Id;
                cmd.Parameters.Add("@LastSeen", SqlDbType.DateTimeOffset);
                cmd.Parameters["@LastSeen"].Value = DateTimeOffset.FromUnixTimeSeconds(data.Timestamp);

                var rows = await cmd.ExecuteNonQueryAsync();
               log.LogInformation($"{rows} rows were updated");
            }
        }
    }
}