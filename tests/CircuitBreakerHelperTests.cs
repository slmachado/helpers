using FluentAssertions;
using Helpers;

namespace Helpers.Tests;

public class CircuitBreakerHelperTests
{
    [Fact]
    public void Execute_Success_StateRemainsOrBecomeClosed()
    {
        var cb = new CircuitBreakerHelper(failureThreshold: 3);
        cb.Execute(() => { });
        cb.State.Should().Be(CircuitState.Closed);
    }

    [Fact]
    public void Execute_FailsUpToThreshold_OpensCircuit()
    {
        var cb = new CircuitBreakerHelper(failureThreshold: 3);

        for (int i = 0; i < 3; i++)
        {
            var act = () => cb.Execute(() => throw new Exception("fail"));
            act.Should().Throw<Exception>();
        }

        cb.State.Should().Be(CircuitState.Open);
    }

    [Fact]
    public void Execute_WhenOpen_ThrowsCircuitOpenException()
    {
        var cb = new CircuitBreakerHelper(failureThreshold: 1);
        var act = () => cb.Execute(() => throw new Exception("fail"));
        act.Should().Throw<Exception>();

        var blockedAct = () => cb.Execute(() => { });
        blockedAct.Should().Throw<CircuitOpenException>();
    }

    [Fact]
    public void Execute_Generic_ReturnsValueOnSuccess()
    {
        var cb = new CircuitBreakerHelper();
        var result = cb.Execute(() => 42);
        result.Should().Be(42);
    }

    [Fact]
    public async Task ExecuteAsync_Success_StateRemainsOrBecomeClosed()
    {
        var cb = new CircuitBreakerHelper(failureThreshold: 3);
        await cb.ExecuteAsync(async () => await Task.CompletedTask);
        cb.State.Should().Be(CircuitState.Closed);
    }

    [Fact]
    public async Task ExecuteAsync_FailsUpToThreshold_OpensCircuit()
    {
        var cb = new CircuitBreakerHelper(failureThreshold: 2);

        for (int i = 0; i < 2; i++)
        {
            var act = () => cb.ExecuteAsync(() => throw new IOException());
            await act.Should().ThrowAsync<IOException>();
        }

        cb.State.Should().Be(CircuitState.Open);
    }

    [Fact]
    public void Reset_ClosesCircuit()
    {
        var cb = new CircuitBreakerHelper(failureThreshold: 1);
        var act = () => cb.Execute(() => throw new Exception());
        act.Should().Throw<Exception>();
        cb.State.Should().Be(CircuitState.Open);

        cb.Reset();
        cb.State.Should().Be(CircuitState.Closed);
    }

    [Fact]
    public void State_AfterOpenDurationPasses_BecomesHalfOpen()
    {
        var cb = new CircuitBreakerHelper(failureThreshold: 1, openDuration: TimeSpan.FromMilliseconds(50));
        var act = () => cb.Execute(() => throw new Exception());
        act.Should().Throw<Exception>();
        cb.State.Should().Be(CircuitState.Open);

        Thread.Sleep(100);
        cb.State.Should().Be(CircuitState.HalfOpen);
    }

    [Fact]
    public void Constructor_InvalidThreshold_Throws()
    {
        var act = () => new CircuitBreakerHelper(failureThreshold: 0);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
