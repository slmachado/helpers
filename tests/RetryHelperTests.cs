using FluentAssertions;
using Helpers;

namespace Helpers.Tests;

public class RetryHelperTests
{
    [Fact]
    public void Execute_SucceedsFirstAttempt_CallsActionOnce()
    {
        int callCount = 0;
        RetryHelper.Execute(() => callCount++, maxRetries: 3);
        callCount.Should().Be(1);
    }

    [Fact]
    public void Execute_FailsTwiceThenSucceeds_CallsActionThreeTimes()
    {
        int callCount = 0;
        RetryHelper.Execute(() =>
        {
            callCount++;
            if (callCount < 3) throw new InvalidOperationException("transient");
        }, maxRetries: 3, delay: TimeSpan.FromMilliseconds(1));

        callCount.Should().Be(3);
    }

    [Fact]
    public void Execute_ExceedsMaxRetries_ThrowsLastException()
    {
        int callCount = 0;
        var act = () => RetryHelper.Execute(() =>
        {
            callCount++;
            throw new InvalidOperationException("always fails");
        }, maxRetries: 2, delay: TimeSpan.FromMilliseconds(1));

        act.Should().Throw<InvalidOperationException>();
        callCount.Should().Be(3); // 1 original + 2 retries
    }

    [Fact]
    public void Execute_Generic_ReturnsResultOnSuccess()
    {
        var result = RetryHelper.Execute(() => 42, maxRetries: 2);
        result.Should().Be(42);
    }

    [Fact]
    public void Execute_OnRetry_CallbackIsInvoked()
    {
        var retryAttempts = new List<int>();

        var act = () => RetryHelper.Execute(
            () => throw new Exception("fail"),
            maxRetries: 2,
            delay: TimeSpan.FromMilliseconds(1),
            onRetry: (_, attempt) => retryAttempts.Add(attempt));

        act.Should().Throw<Exception>();
        retryAttempts.Should().BeEquivalentTo([1, 2]);
    }

    [Fact]
    public void Execute_MaxRetriesZero_Throws()
    {
        var act = () => RetryHelper.Execute(() => { }, maxRetries: 0);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public async Task ExecuteAsync_SucceedsFirstAttempt_CallsFuncOnce()
    {
        int callCount = 0;
        await RetryHelper.ExecuteAsync(async () =>
        {
            callCount++;
            await Task.CompletedTask;
        }, maxRetries: 2);

        callCount.Should().Be(1);
    }

    [Fact]
    public async Task ExecuteAsync_FailsTwiceThenSucceeds()
    {
        int callCount = 0;
        await RetryHelper.ExecuteAsync(async () =>
        {
            callCount++;
            if (callCount < 3) throw new IOException("network error");
            await Task.CompletedTask;
        }, maxRetries: 3, delay: TimeSpan.FromMilliseconds(1));

        callCount.Should().Be(3);
    }

    [Fact]
    public async Task ExecuteAsync_Generic_ReturnsValue()
    {
        var result = await RetryHelper.ExecuteAsync(
            () => Task.FromResult("ok"), maxRetries: 2);

        result.Should().Be("ok");
    }

    [Fact]
    public async Task ExecuteWithExponentialBackoffAsync_SucceedsAfterRetries()
    {
        int callCount = 0;
        await RetryHelper.ExecuteWithExponentialBackoffAsync(async () =>
        {
            callCount++;
            if (callCount < 3) throw new TimeoutException();
            await Task.CompletedTask;
        }, maxRetries: 5, initialDelay: TimeSpan.FromMilliseconds(1));

        callCount.Should().Be(3);
    }

    [Fact]
    public async Task ExecuteWithExponentialBackoffAsync_ExceedsMaxRetries_Throws()
    {
        var act = () => RetryHelper.ExecuteWithExponentialBackoffAsync(
            () => throw new Exception("always fails"),
            maxRetries: 2,
            initialDelay: TimeSpan.FromMilliseconds(1));

        await act.Should().ThrowAsync<Exception>();
    }
}
