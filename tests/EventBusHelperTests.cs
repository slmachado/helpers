using FluentAssertions;
using Helpers;

namespace Helpers.Tests;

public class EventBusHelperTests
{
    [Fact]
    public void Publish_SyncHandler_IsInvoked()
    {
        var bus = new EventBusHelper();
        int received = 0;

        bus.Subscribe<string>(msg => received++);
        bus.Publish("hello");

        received.Should().Be(1);
    }

    [Fact]
    public void Publish_MultipleHandlers_AllInvoked()
    {
        var bus = new EventBusHelper();
        int count = 0;

        bus.Subscribe<int>(_ => count++);
        bus.Subscribe<int>(_ => count++);
        bus.Publish(42);

        count.Should().Be(2);
    }

    [Fact]
    public void Publish_NoHandlers_DoesNotThrow()
    {
        var bus = new EventBusHelper();
        var act = () => bus.Publish("no listeners");
        act.Should().NotThrow();
    }

    [Fact]
    public async Task PublishAsync_AsyncHandler_IsInvoked()
    {
        var bus = new EventBusHelper();
        int received = 0;

        bus.Subscribe<string>(async msg =>
        {
            received++;
            await Task.CompletedTask;
        });

        await bus.PublishAsync("hello async");
        received.Should().Be(1);
    }

    [Fact]
    public async Task PublishAsync_MixedHandlers_BothInvoked()
    {
        var bus = new EventBusHelper();
        var results = new List<string>();

        bus.Subscribe<string>(msg => results.Add("sync:" + msg));
        bus.Subscribe<string>(async msg =>
        {
            results.Add("async:" + msg);
            await Task.CompletedTask;
        });

        await bus.PublishAsync("event");
        results.Should().Contain("sync:event").And.Contain("async:event");
    }

    [Fact]
    public void Unsubscribe_RemovesAllHandlers()
    {
        var bus = new EventBusHelper();
        int count = 0;

        bus.Subscribe<string>(_ => count++);
        bus.Unsubscribe<string>();
        bus.Publish("should not trigger");

        count.Should().Be(0);
    }

    [Fact]
    public void HandlerCount_ReturnsCorrectCount()
    {
        var bus = new EventBusHelper();
        bus.Subscribe<double>(_ => { });
        bus.Subscribe<double>(_ => { });

        bus.HandlerCount<double>().Should().Be(2);
        bus.HandlerCount<string>().Should().Be(0);
    }

    [Fact]
    public void Clear_RemovesEverything()
    {
        var bus = new EventBusHelper();
        bus.Subscribe<string>(_ => { });
        bus.Subscribe<int>(_ => { });

        bus.Clear();

        bus.HandlerCount<string>().Should().Be(0);
        bus.HandlerCount<int>().Should().Be(0);
    }
}
