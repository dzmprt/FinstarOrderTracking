using System.Net.WebSockets;
using Confluent.Kafka;
using FOT.OrderChangedWorker;
using FOT.OrderChangedWorker.Abstractions;
using FOT.OrderChangedWorker.Infrastructure;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<WebSocketConnectionManager>();
builder.Services.AddSingleton<IOrderNotifier, WebSocketOrderNotifier>();
builder.Services.AddHostedService<KafkaWorker>();
builder.Services.Configure<ConsumerConfig>(
    builder.Configuration.GetSection("Kafka"));
builder.Services.AddSingleton<KafkaConsumerFactory>();
builder.Services.AddSingleton<KafkaMessageHandler>();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService("FOT.OrderChangedWorker"))
    .WithTracing(tp => tp
        .AddAspNetCoreInstrumentation(options => options.RecordException = true)
        .AddHttpClientInstrumentation()
        .AddOtlpExporter());

var app = builder.Build();



app.UseHttpsRedirection();

app.UseWebSockets();

app.Map("/orders-statuses", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        var id = Guid.NewGuid().ToString();
        var manager = context.RequestServices.GetRequiredService<WebSocketConnectionManager>();
        manager.AddSocket(id, socket);

        while (socket.State == WebSocketState.Open)
        {
            await Task.Delay(1000);
        }

        manager.RemoveSocket(id);
    }
    else
    {
        context.Response.StatusCode = StatusCodes.Status426UpgradeRequired;
        context.Response.Headers["Upgrade"] = "websocket";
        context.Response.Headers["Connection"] = "Upgrade";
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync("This service requires use of the WebSocket protocol.");
    }
});


app.Run();
