using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Net.Sockets;
using System.Net;
using WebSocketSharp.Server;
using static EmotionDetectionServer.API.EdsController;
using EmotionDetectionSystem.ServiceLayer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache(); // Add this line to configure session storage
builder.Services.AddSession(options =>
{
    // Configure session options as needed
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
builder.Services.AddControllers();

WebSocketServer notificationServer = new WebSocketServer($"ws://{GetLocalIPAddress()}:" + "7172");
WebSocketServer logsServer = new WebSocketServer(System.Net.IPAddress.Parse("127.0.0.1"), 4560);
EdsService service = new EdsService();
logsServer.AddWebSocketService<logsService>("/logs");
notificationServer.Start();
logsServer.Start();
builder.Services.AddSingleton(_ => notificationServer);
builder.Services.AddSingleton(_ => logsServer);
builder.Services.AddSingleton<IEdsService, EdsService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

static string GetLocalIPAddress()
{
    var host = Dns.GetHostEntry(Dns.GetHostName());
    foreach (var ip in host.AddressList)
    {
        if (ip.AddressFamily == AddressFamily.InterNetwork)
        {
            return ip.ToString();
        }
    }
    throw new Exception("No network adapters with an IPv4 address in the system!");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowOrigin"); // Apply CORS policy
app.UseSession(); // Use session middleware before routing middleware
app.UseAuthorization();
app.MapControllers();
app.MapRazorPages();

app.Run();