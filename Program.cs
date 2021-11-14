using FH5RP.Data;
using FH5RP.Hubs;
using FH5RP.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

TelemetryServer Server = new TelemetryServer();
Server.Start();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseResponseCompression();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapControllers();
app.MapHub<TelemetryDataHub>("/datahub");
app.MapFallbackToPage("/_Host");

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
IHubContext<TelemetryDataHub> datahub = (IHubContext<TelemetryDataHub>)app.Services.GetService(typeof(IHubContext<TelemetryDataHub>));
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

#pragma warning disable CS8602 // Dereference of a possibly null reference.
Server.OnDataUpdated += async (data) => await datahub?.Clients.All.SendAsync("ReceiveData", data);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

app.Run();
