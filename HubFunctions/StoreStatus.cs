namespace HubFunctions;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Threading.Tasks;

public class StoreStatus
{
    private readonly IConfiguration _config;
    private readonly ILogger<StoreStatus> _log;

    public StoreStatus(IConfiguration config, ILogger<StoreStatus> log)
    {
        _config = config;
        _log = log;
    }

    [Function("storeStatus")]
    [SignalROutput(HubName = "dotdev")]
    public async Task<SignalRMessageAction?> Run(
        [QueueTrigger("elementstatus")] StatusDatapoint data,
        FunctionContext context)
    {
        _log.LogInformation("Processing status update for ID {Id}", data.Id);

        await UpdateDatabase(data);

        // Return SignalR message back to the hub
        return new SignalRMessageAction("elementstatus")
        {
            Arguments = new object[] { data.Id, data.Timestamp }
        };
    }

    private async Task UpdateDatabase(StatusDatapoint data)
    {
        var str = _config.GetConnectionString("dotdev_cs");
        if (str is null)
        {
            _log.LogError("Database connection string 'dotdev_cs' is missing.");
            return;
        }

        using var conn = new SqlConnection(str);
        await conn.OpenAsync();

        using var cmd = new SqlCommand("dd_ServerUpdate", conn)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.Add("@Number", SqlDbType.Int).Value = data.Id;
        cmd.Parameters.Add("@LastSeen", SqlDbType.DateTimeOffset)
                      .Value = DateTimeOffset.FromUnixTimeSeconds(data.Timestamp);

        var rows = await cmd.ExecuteNonQueryAsync();
        _log.LogInformation("{rows} rows were updated.", rows);
    }
}