using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DotDev.Client;
using DotDev.Core.HexPath;
using DotDev.Client.Hubs;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var config = builder.Configuration;

var apiBase = config["ApiBase"] ?? throw new Exception("ApiBase missing in configuration.");
Console.WriteLine(apiBase);

var hubUrl = config["HubUrl"]  ?? throw new Exception("HubUrl missing in configuration.");
Console.WriteLine(hubUrl);

builder.Services.AddSingleton(new HubConfig { HubUrl = hubUrl });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBase) });
builder.Services.AddScoped<IHoneycomb, Honeycomb>();

await builder.Build().RunAsync();


public sealed class EndpointConfig
{
    public required string ApiBase { get; init; }
    public required string HubUrl { get; init; }
}
