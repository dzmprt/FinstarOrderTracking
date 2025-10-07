using Confluent.Kafka;
using FOT.OutboxWorker;
using FOT.OutboxWorker.Abstractions;
using FOT.OutboxWorker.Infrastructure;
using Npgsql;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService("FOT.OutboxWorker"))
    .WithTracing(tp => tp
        .AddHttpClientInstrumentation()
        .AddNpgsql()
        .AddOtlpExporter());
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IProducer<string, string>>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var kafkaConfig = new ProducerConfig();
    config.GetSection("Kafka").Bind(kafkaConfig);

    var producer = new ProducerBuilder<string, string>(kafkaConfig).Build();
    producer.InitTransactions(TimeSpan.FromSeconds(10));
    return producer;
});

builder.Services.AddSingleton<IOutboxPublisher, KafkaOutboxPublisher>();

var host = builder.Build();
host.Run();
