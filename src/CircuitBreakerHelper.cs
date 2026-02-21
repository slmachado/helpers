namespace Helpers;

/// <summary>
/// Represents the state of a circuit breaker.
/// </summary>
public enum CircuitState
{
    /// <summary>Requests are allowed through normally.</summary>
    Closed,
    /// <summary>Requests are blocked — too many recent failures.</summary>
    Open,
    /// <summary>A single probe request is allowed to test recovery.</summary>
    HalfOpen
}

/// <summary>
/// Implements the Circuit Breaker pattern to prevent cascading failures.
/// Thread-safe via a lock.
/// </summary>
public sealed class CircuitBreakerHelper
{
    private readonly int _failureThreshold;
    private readonly TimeSpan _openDuration;
    private readonly object _lock = new();

    private int _failureCount;
    private CircuitState _state = CircuitState.Closed;
    private DateTime _openedAt = DateTime.MinValue;

    /// <summary>Gets the current circuit state.</summary>
    public CircuitState State
    {
        get
        {
            lock (_lock) return GetCurrentState();
        }
    }

    /// <param name="failureThreshold">Number of consecutive failures before opening the circuit.</param>
    /// <param name="openDuration">How long the circuit stays open before transitioning to HalfOpen.</param>
    public CircuitBreakerHelper(int failureThreshold = 3, TimeSpan? openDuration = null)
    {
        if (failureThreshold < 1)
            throw new ArgumentOutOfRangeException(nameof(failureThreshold), "Must be at least 1.");

        _failureThreshold = failureThreshold;
        _openDuration = openDuration ?? TimeSpan.FromSeconds(30);
    }

    /// <summary>
    /// Executes an action through the circuit breaker.
    /// Throws <see cref="CircuitOpenException"/> if the circuit is open.
    /// </summary>
    public void Execute(Action action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        lock (_lock)
        {
            ThrowIfOpen();
        }

        try
        {
            action();
            RecordSuccess();
        }
        catch (Exception)
        {
            RecordFailure();
            throw;
        }
    }

    /// <summary>
    /// Executes a function through the circuit breaker and returns its result.
    /// Throws <see cref="CircuitOpenException"/> if the circuit is open.
    /// </summary>
    public T Execute<T>(Func<T> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        lock (_lock)
        {
            ThrowIfOpen();
        }

        try
        {
            var result = func();
            RecordSuccess();
            return result;
        }
        catch (Exception)
        {
            RecordFailure();
            throw;
        }
    }

    /// <summary>
    /// Executes an async function through the circuit breaker.
    /// Throws <see cref="CircuitOpenException"/> if the circuit is open.
    /// </summary>
    public async Task ExecuteAsync(Func<Task> func, CancellationToken cancellationToken = default)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        lock (_lock)
        {
            ThrowIfOpen();
        }

        try
        {
            await func();
            RecordSuccess();
        }
        catch (Exception)
        {
            RecordFailure();
            throw;
        }
    }

    /// <summary>
    /// Executes an async function through the circuit breaker and returns its result.
    /// </summary>
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> func, CancellationToken cancellationToken = default)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        lock (_lock)
        {
            ThrowIfOpen();
        }

        try
        {
            var result = await func();
            RecordSuccess();
            return result;
        }
        catch (Exception)
        {
            RecordFailure();
            throw;
        }
    }

    /// <summary>Resets the circuit breaker to the Closed state.</summary>
    public void Reset()
    {
        lock (_lock)
        {
            _failureCount = 0;
            _state = CircuitState.Closed;
        }
    }

    private CircuitState GetCurrentState()
    {
        if (_state == CircuitState.Open && DateTime.UtcNow - _openedAt >= _openDuration)
        {
            _state = CircuitState.HalfOpen;
        }
        return _state;
    }

    private void ThrowIfOpen()
    {
        if (GetCurrentState() == CircuitState.Open)
            throw new CircuitOpenException("Circuit is open. Calls are not allowed.");
    }

    private void RecordSuccess()
    {
        lock (_lock)
        {
            _failureCount = 0;
            _state = CircuitState.Closed;
        }
    }

    private void RecordFailure()
    {
        lock (_lock)
        {
            _failureCount++;
            if (_failureCount >= _failureThreshold)
            {
                _state = CircuitState.Open;
                _openedAt = DateTime.UtcNow;
            }
        }
    }
}

/// <summary>
/// Exception thrown when a call is attempted while the circuit is open.
/// </summary>
public sealed class CircuitOpenException(string message) : Exception(message);
