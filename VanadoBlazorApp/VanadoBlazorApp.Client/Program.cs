using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VanadoBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<KvaroviHubService>();

await builder.Build().RunAsync();
