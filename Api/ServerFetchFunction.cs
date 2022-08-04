namespace DotDev.Api
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Dapper;
    using Dotdev.Core.Element;

    public static class ServerFetchFunction
    {
        [FunctionName("ServerFetch")]
        public async static Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log, 
            ExecutionContext context)
        {
            log.LogInformation("ServerFetch!");

            var connStr = Environment.GetEnvironmentVariable("dotdev_cs");
            //var config = new ConfigurationBuilder()
            //    .SetBasePath(context.FunctionAppDirectory)
            //    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            //    .AddEnvironmentVariables()
            //    .Build();
            //var connStr = config.GetConnectionString("dotdev");

            var fetched = await FetchAsync<ServerInfo>(connStr, log);
            log.LogInformation("Returned from DB for ServerFetch {retrieved}", fetched?.Count());
           // var result = fetched ?? GetExampleServers() ?? new List<ServerInfo>();
            //var result = GetExampleServers() ?? new List<ServerInfo>();
            return new OkObjectResult(fetched);
        }

        private static IEnumerable<ServerInfo> GetExampleServers()
        {
            var h = new ServerInfo()
            {
                Number = 1,
                Name = "Hydrogen",
                LastStatus = "online",
                DeviceType = 1,
                Symbol = "H",
                LastSeen = DateTimeOffset.UtcNow.AddSeconds(-30)
            };
            var he = new ServerInfo()
            {
                Number = 2,
                Name = "Helium",
                LastStatus = "untracked",
                DeviceType = 1,
                Symbol = "He",
                LastSeen = DateTimeOffset.UtcNow.AddSeconds(-30)
            };
            //var li = new ServerInfo()
            //{
            //    Number = 3,
            //    Name = "Lithium",
            //    LastStatus = "offline",
            //    DeviceType = 2,
            //    IP = ".3",
            //    Symbol = "Li",
            //    LastSeen = DateTimeOffset.UtcNow.AddMonths(-5)
            //};
            var c = new ServerInfo()
            {
                Number = 6,
                Name = "Carbon",
                LastStatus = "untracked",
                DeviceType = 1,
                Symbol = "C",
                LastSeen = DateTimeOffset.UtcNow.AddSeconds(-30)
            };
            //var cl = new ServerInfo()
            //{
            //    Number = 17,
            //    Name = "Chlorine",
            //    LastStatus = "online",
            //    DeviceType = 4,
            //    IP = ".17",
            //    Symbol = "Cl",
            //    LastSeen = DateTimeOffset.UtcNow.AddSeconds(-5)
            //};
            //var k = new ServerInfo()
            //{
            //    Number = 19,
            //    Name = "Potassium",
            //    LastStatus = "delayed",
            //    DeviceType = 3,
            //    IP = ".19",
            //    Symbol = "K",
            //    LastSeen = DateTimeOffset.UtcNow.AddSeconds(-625)
            //};
            var ni = new ServerInfo()
            {
                Number = 28,
                Name = "Nickel",
                LastStatus = "untracked",
                DeviceType = 6,
                Symbol = "Ni",
                LastSeen = DateTimeOffset.UtcNow.AddSeconds(-134)
            };
            var cu = new ServerInfo()
            {
                Number = 29,
                Name = "Copper",
                LastStatus = "offline",
                DeviceType = 7,
                Symbol = "Cu",
                LastSeen = DateTimeOffset.UtcNow.AddSeconds(-324)
            };
            var pd = new ServerInfo()
            {
                Number = 46,
                Name = "Palladium",
                LastStatus = "untracked",
                DeviceType = 7,
                Symbol = "Pd",
                LastSeen = DateTimeOffset.UtcNow.AddMinutes(-53)
            };
            var ag = new ServerInfo()
            {
                Number = 47,
                Name = "Silver",
                LastStatus = "untracked",
                DeviceType = 7,
                Symbol = "Ag",
                LastSeen = DateTimeOffset.UtcNow.AddMinutes(-19)
            };
            //var io = new ServerInfo()
            //{
            //    Number = 53,
            //    Name = "Iodine",
            //    LastStatus = "untracked",
            //    DeviceType = 4,
            //    IP = ".53",
            //    Symbol = "I",
            //    LastSeen = DateTimeOffset.UtcNow.AddMinutes(-438)
            //};
            var xe = new ServerInfo()
            {
                Number = 54,
                Name = "Xenon",
                LastStatus = "untracked",
                DeviceType = 9,
                Symbol = "Xe",
                LastSeen = DateTimeOffset.UtcNow.AddSeconds(-5)
            };
            //var ho = new ServerInfo()
            //{
            //    Number = 65,
            //    Name = "Holmium",
            //    LastStatus = "untracked",
            //    DeviceType = 8,
            //    IP = ".65",
            //    Symbol = "Ho",
            //    LastSeen = DateTimeOffset.UtcNow.AddMinutes(-2252)
            //};

            var result = new List<ServerInfo>();
            result.Add(h);
            result.Add(he);
            //result.Add(li);
            result.Add(c);
            //result.Add(cl);
            //result.Add(k);
            result.Add(ni);
            result.Add(cu);
            result.Add(pd);
            result.Add(ag);
            //result.Add(io);
            result.Add(xe);
            //result.Add(ho);
            return result;
        }

        private static async Task<IEnumerable<T>> FetchAsync<T>(string connStr, ILogger log)
        {
            var query = "select * from view_Server";

            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    log.LogInformation("connected to DB for ServerFetch!");
                    return await conn.QueryAsync<T>(query, commandType: CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "ElementFetch DB fail");
                throw;
            }
        }
    }
}
