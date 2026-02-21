using FluentAssertions;
using Helpers;

namespace Helpers.Tests;

public class BackgroundTaskQueueTests
{
    [Fact]
    public async Task EnqueueAsync_And_DequeueAsync_RoundTrip()
    {
        var queue = new BackgroundTaskQueue(10);
        int executed = 0;

        await queue.EnqueueAsync(async ct =>
        {
            executed++;
            await ValueTask.CompletedTask;
        });

        var item = await queue.DequeueAsync();
        await item(CancellationToken.None);

        executed.Should().Be(1);
    }

    [Fact]
    public async Task PendingCount_ReflectsEnqueuedItems()
    {
        var queue = new BackgroundTaskQueue(10);

        await queue.EnqueueAsync(_ => ValueTask.CompletedTask);
        await queue.EnqueueAsync(_ => ValueTask.CompletedTask);

        queue.PendingCount.Should().Be(2);
    }

    [Fact]
    public async Task ReadAllAsync_ConsumesAllItems()
    {
        var queue = new BackgroundTaskQueue(10);
        int count = 0;

        for (int i = 0; i < 3; i++)
            await queue.EnqueueAsync(_ => ValueTask.CompletedTask);

        queue.Complete();

        await foreach (var item in queue.ReadAllAsync())
        {
            await item(CancellationToken.None);
            count++;
        }

        count.Should().Be(3);
    }

    [Fact]
    public async Task EnqueueAsync_NullItem_Throws()
    {
        var queue = new BackgroundTaskQueue(10);
        Func<Task> act = () => queue.EnqueueAsync(null!).AsTask();
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
