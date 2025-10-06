using FOT.Domain.Common;

namespace FOT.Domain.Tests;

public class BaseDomainTests
{
    private class TestDomainEvent : BaseDomainEvent
    {
        public TestDomainEvent(DateTimeOffset dt) : base(dt) { }
    }

    [Fact]
    public void AddDomainEvent_AddsEvent()
    {
        var domain = new BaseDomain();
        var evt = new TestDomainEvent(DateTimeOffset.UtcNow);
        domain.AddDomainEvent(evt);
        Assert.Contains(evt, domain.GetDomainEvents());
    }

    [Fact]
    public void RemoveDomainEvent_RemovesEvent()
    {
        var domain = new BaseDomain();
        var evt = new TestDomainEvent(DateTimeOffset.UtcNow);
        domain.AddDomainEvent(evt);
        domain.RemoveDomainEvent(evt);
        Assert.DoesNotContain(evt, domain.GetDomainEvents());
    }

    [Fact]
    public void ClearDomainEvents_RemovesAll()
    {
        var domain = new BaseDomain();
        domain.AddDomainEvent(new TestDomainEvent(DateTimeOffset.UtcNow));
        domain.AddDomainEvent(new TestDomainEvent(DateTimeOffset.UtcNow));
        domain.ClearDomainEvents();
        Assert.Empty(domain.GetDomainEvents());
    }
}