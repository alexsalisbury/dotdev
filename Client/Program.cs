using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DotDev.Client;
using DotDev.Core.HexPath;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
//builder.Configuration.AddJsonFile("appsettings.json");
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
    var baseUri = new Uri(new Uri(builder.HostEnvironment.BaseAddress), "api/");
    return new HttpClient { BaseAddress = baseUri };
});


// Your Honeycomb engine stays DI‑based
builder.Services.AddScoped<IHoneycomb, Honeycomb>();

await builder.Build().RunAsync();