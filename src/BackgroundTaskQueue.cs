using System.Threading.Channels;

namespace Helpers;

/// <summary>
/// A thread-safe queue for scheduling background work items as async delegates.
/// Intended to be registered as a singleton and consumed by a hosted background service.
/// </summary>
public sealed class BackgroundTaskQueue
{
    private readonly Channel<Func<CancellationToken, ValueTask>> _queue;

    /// <param name="capacity">Maximum number of pending tasks. Defaults to 100.</param>
    public BackgroundTaskQueue(int capacity = 100)
    {
        var options = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = false
        };

        _queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);
    }

    /// <summary>
    /// Enqueues a work item to be executed in the background.
    /// </summary>
    /// <param name="workItem">The async work item to enqueue.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async ValueTask EnqueueAsync(
        Func<CancellationToken, ValueTask> workItem,
        CancellationToken cancellationToken = default)
    {
        if (workItem == null) throw new ArgumentNullException(nameof(workItem));
        await _queue.Writer.WriteAsync(workItem, cancellationToken);
    }

    /// <summary>
    /// Dequeues and returns the next available work item, waiting if the queue is empty.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(
        CancellationToken cancellationToken = default)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }

    /// <summary>
    /// Returns the count of pending items currently in the queue.
    /// </summary>
    public int PendingCount => _queue.Reader.Count;

    /// <summary>
    /// Signals that no more items will be enqueued.
    /// </summary>
    public void Complete() => _queue.Writer.Complete();

    /// <summary>
    /// Reads all pending work items as an async enumerable.
    /// </summary>
    public IAsyncEnumerable<Func<CancellationToken, ValueTask>> ReadAllAsync(
        CancellationToken cancellationToken = default)
        => _queue.Reader.ReadAllAsync(cancellationToken);
}
