using FOT.Domain.Common;

namespace FOT.Domain.Tests;

public class BaseDomainEventTests
{
    [Fact]
    public void Constructor_SetsEventCreatedAt()
    {
        var now = DateTimeOffset.UtcNow;
        var evt = new TestDomainEvent(now);
        Assert.Equal(now, evt.EventCreatedAt);
    }

    private class TestDomainEvent : BaseDomainEvent
    {
        public TestDomainEvent(DateTimeOffset dt) : base(dt) { }
    }
}