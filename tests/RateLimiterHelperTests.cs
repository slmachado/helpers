using FluentAssertions;
using Helpers;

namespace Helpers.Tests;

public class RateLimiterHelperTests
{
    [Fact]
    public void TryAcquire_WithinLimit_ReturnsTrue()
    {
        var limiter = new RateLimiterHelper(maxOperations: 5);

        for (int i = 0; i < 5; i++)
            limiter.TryAcquire().Should().BeTrue();
    }

    [Fact]
    public void TryAcquire_ExceedsLimit_ReturnsFalse()
    {
        var limiter = new RateLimiterHelper(maxOperations: 3);

        limiter.TryAcquire();
        limiter.TryAcquire();
        limiter.TryAcquire();

        limiter.TryAcquire().Should().BeFalse();
    }

    [Fact]
    public void TryAcquire_WithCount_ConsumesMultipleTokens()
    {
        var limiter = new RateLimiterHelper(maxOperations: 5);
        limiter.TryAcquire(3).Should().BeTrue();
        limiter.AvailableTokens.Should().Be(2);
    }

    [Fact]
    public void TryAcquire_WithCountExceedingTokens_ReturnsFalse()
    {
        var limiter = new RateLimiterHelper(maxOperations: 2);
        limiter.TryAcquire(3).Should().BeFalse();
    }

    [Fact]
    public void Execute_WithinLimit_Executes()
    {
        var limiter = new RateLimiterHelper(maxOperations: 5);
        int callCount = 0;

        limiter.Execute(() => callCount++);

        callCount.Should().Be(1);
    }

    [Fact]
    public void Execute_ExceedsLimit_ThrowsRateLimitExceededException()
    {
        var limiter = new RateLimiterHelper(maxOperations: 1);
        limiter.TryAcquire(); // exhaust

        var act = () => limiter.Execute(() => { });
        act.Should().Throw<RateLimitExceededException>();
    }

    [Fact]
    public async Task ExecuteAsync_WithinLimit_Executes()
    {
        var limiter = new RateLimiterHelper(maxOperations: 5);
        bool ran = false;

        await limiter.ExecuteAsync(async () =>
        {
            ran = true;
            await Task.CompletedTask;
        });

        ran.Should().BeTrue();
    }

    [Fact]
    public async Task ExecuteAsync_ExceedsLimit_Throws()
    {
        var limiter = new RateLimiterHelper(maxOperations: 1);
        limiter.TryAcquire();

        var act = () => limiter.ExecuteAsync(() => Task.CompletedTask);
        await act.Should().ThrowAsync<RateLimitExceededException>();
    }

    [Fact]
    public void TokensRefill_AfterInterval()
    {
        var limiter = new RateLimiterHelper(maxOperations: 2, interval: TimeSpan.FromMilliseconds(50));
        limiter.TryAcquire();
        limiter.TryAcquire();
        limiter.AvailableTokens.Should().Be(0);

        Thread.Sleep(100);

        limiter.AvailableTokens.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Constructor_InvalidMaxOperations_Throws()
    {
        var act = () => new RateLimiterHelper(0);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
