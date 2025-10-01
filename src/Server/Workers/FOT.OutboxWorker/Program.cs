using Confluent.Kafka;
using FOT.OutboxWorker;
using FOT.OutboxWorker.Abstractions;
using FOT.OutboxWorker.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);
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