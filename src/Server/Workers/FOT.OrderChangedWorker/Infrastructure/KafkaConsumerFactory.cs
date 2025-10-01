using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace FOT.OrderChangedWorker.Infrastructure;

public class KafkaConsumerFactory(IOptions<ConsumerConfig> config)
{
    public IConsumer<Ignore, string> CreateConsumer()
    {
        return new ConsumerBuilder<Ignore, string>(config.Value).Build();
    }
}