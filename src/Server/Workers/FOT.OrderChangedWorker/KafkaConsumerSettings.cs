namespace FOT.OrderChangedWorker;

public class KafkaConsumerSettings
{
    public string BootstrapServers { get; set; } = default!;
    public string ClientId { get; set; } = default!;
    public string SecurityProtocol { get; set; } = "PLAINTEXT";
    public string GroupId { get; set; } = default!;
    public string AutoOffsetReset { get; set; } = "earliest";
    public string OrderStateTopic { get; set; } = default!;
}