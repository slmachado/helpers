namespace Helpers;

/// <summary>
/// A lightweight in-process publish/subscribe event bus.
/// Handlers are identified by event type. Thread-safe.
/// </summary>
public sealed class EventBusHelper
{
    private readonly Dictionary<Type, List<Delegate>> _handlers = new();
    private readonly object _lock = new();

    /// <summary>
    /// Subscribes a synchronous handler to events of type <typeparamref name="TEvent"/>.
    /// </summary>
    /// <typeparam name="TEvent">The event type to subscribe to.</typeparam>
    /// <param name="handler">The handler to invoke when the event is published.</param>
    public void Subscribe<TEvent>(Action<TEvent> handler)
    {
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        lock (_lock)
        {
            if (!_handlers.TryGetValue(typeof(TEvent), out var list))
            {
                list = [];
                _handlers[typeof(TEvent)] = list;
            }
            list.Add(handler);
        }
    }

    /// <summary>
    /// Subscribes an asynchronous handler to events of type <typeparamref name="TEvent"/>.
    /// </summary>
    /// <typeparam name="TEvent">The event type to subscribe to.</typeparam>
    /// <param name="handler">The async handler to invoke when the event is published.</param>
    public void Subscribe<TEvent>(Func<TEvent, Task> handler)
    {
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        lock (_lock)
        {
            if (!_handlers.TryGetValue(typeof(TEvent), out var list))
            {
                list = [];
                _handlers[typeof(TEvent)] = list;
            }
            list.Add(handler);
        }
    }

    /// <summary>
    /// Removes all handlers for events of type <typeparamref name="TEvent"/>.
    /// </summary>
    public void Unsubscribe<TEvent>()
    {
        lock (_lock)
        {
            _handlers.Remove(typeof(TEvent));
        }
    }

    /// <summary>
    /// Publishes an event synchronously to all registered sync handlers.
    /// Async handlers are not invoked by this method.
    /// </summary>
    /// <typeparam name="TEvent">The event type.</typeparam>
    /// <param name="event">The event instance to publish.</param>
    public void Publish<TEvent>(TEvent @event)
    {
        if (@event == null) throw new ArgumentNullException(nameof(@event));

        List<Delegate> snapshot;
        lock (_lock)
        {
            if (!_handlers.TryGetValue(typeof(TEvent), out var list)) return;
            snapshot = [.. list];
        }

        foreach (var handler in snapshot)
        {
            if (handler is Action<TEvent> syncHandler)
                syncHandler(@event);
        }
    }

    /// <summary>
    /// Publishes an event asynchronously, invoking both sync and async handlers in order.
    /// </summary>
    /// <typeparam name="TEvent">The event type.</typeparam>
    /// <param name="event">The event instance to publish.</param>
    public async Task PublishAsync<TEvent>(TEvent @event)
    {
        if (@event == null) throw new ArgumentNullException(nameof(@event));

        List<Delegate> snapshot;
        lock (_lock)
        {
            if (!_handlers.TryGetValue(typeof(TEvent), out var list)) return;
            snapshot = [.. list];
        }

        foreach (var handler in snapshot)
        {
            if (handler is Func<TEvent, Task> asyncHandler)
                await asyncHandler(@event);
            else if (handler is Action<TEvent> syncHandler)
                syncHandler(@event);
        }
    }

    /// <summary>
    /// Returns the number of registered handlers for a given event type.
    /// </summary>
    public int HandlerCount<TEvent>()
    {
        lock (_lock)
        {
            return _handlers.TryGetValue(typeof(TEvent), out var list) ? list.Count : 0;
        }
    }

    /// <summary>
    /// Removes all subscriptions from the event bus.
    /// </summary>
    public void Clear()
    {
        lock (_lock) _handlers.Clear();
    }
}
