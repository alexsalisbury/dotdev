using HubFunctions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Threading.Tasks;

public class StoreStatus
{
    private readonly ILogger _logger;
    private readonly IConfiguration _config;

    public StoreStatus(ILoggerFactory loggerFactory, IConfiguration config)
    {
        _logger = loggerFactory.CreateLogger<StoreStatus>();
        _config = config;
    }

    [Function("storeStatus")]
    public async Task Run(
        [QueueTrigger("elementstatus")] StatusDatapoint data,
        FunctionContext context)
    {
        await UpdateDatabase(data);
    }

    private async Task UpdateDatabase(StatusDatapoint data)
    {
        var str = _config.GetConnectionString("dotdev");

        using var conn = new SqlConnection(str);
        await conn.OpenAsync();

        using var cmd = new SqlCommand("dd_ServerUpdate", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@Number", SqlDbType.Int).Value = data.Id;
        cmd.Parameters.Add("@LastSeen", SqlDbType.DateTimeOffset)
            .Value = DateTimeOffset.FromUnixTimeSeconds(data.Timestamp);

        var rows = await cmd.ExecuteNonQueryAsync();
        _logger.LogInformation("{Rows} rows were updated", rows);
    }
}
