namespace Helpers;

/// <summary>
/// A thread-safe token bucket rate limiter.
/// Limits the number of operations permitted within a given time window.
/// </summary>
public sealed class RateLimiterHelper
{
    private readonly int _maxTokens;
    private readonly TimeSpan _refillInterval;
    private readonly object _lock = new();

    private int _currentTokens;
    private DateTime _lastRefill;

    /// <param name="maxOperations">Maximum number of operations allowed per interval.</param>
    /// <param name="interval">The time window for the token bucket. Defaults to 1 second.</param>
    public RateLimiterHelper(int maxOperations, TimeSpan? interval = null)
    {
        if (maxOperations < 1)
            throw new ArgumentOutOfRangeException(nameof(maxOperations), "Must be at least 1.");

        _maxTokens = maxOperations;
        _refillInterval = interval ?? TimeSpan.FromSeconds(1);
        _currentTokens = maxOperations;
        _lastRefill = DateTime.UtcNow;
    }

    /// <summary>
    /// Attempts to consume one token. Returns true if the operation is allowed,
    /// false if the rate limit has been exceeded.
    /// </summary>
    public bool TryAcquire()
    {
        lock (_lock)
        {
            Refill();
            if (_currentTokens <= 0) return false;
            _currentTokens--;
            return true;
        }
    }

    /// <summary>
    /// Attempts to consume a specified number of tokens at once.
    /// </summary>
    /// <param name="count">Number of tokens to consume.</param>
    public bool TryAcquire(int count)
    {
        if (count < 1) throw new ArgumentOutOfRangeException(nameof(count));
        lock (_lock)
        {
            Refill();
            if (_currentTokens < count) return false;
            _currentTokens -= count;
            return true;
        }
    }

    /// <summary>
    /// Executes an action if a token is available; otherwise throws <see cref="RateLimitExceededException"/>.
    /// </summary>
    public void Execute(Action action)
    {
        if (!TryAcquire()) throw new RateLimitExceededException("Rate limit exceeded.");
        action();
    }

    /// <summary>
    /// Executes an async function if a token is available; otherwise throws <see cref="RateLimitExceededException"/>.
    /// </summary>
    public async Task ExecuteAsync(Func<Task> func)
    {
        if (!TryAcquire()) throw new RateLimitExceededException("Rate limit exceeded.");
        await func();
    }

    /// <summary>Gets the number of tokens currently available.</summary>
    public int AvailableTokens
    {
        get { lock (_lock) { Refill(); return _currentTokens; } }
    }

    private void Refill()
    {
        var now = DateTime.UtcNow;
        var elapsed = now - _lastRefill;

        if (elapsed >= _refillInterval)
        {
            var periods = (int)(elapsed.TotalMilliseconds / _refillInterval.TotalMilliseconds);
            _currentTokens = Math.Min(_maxTokens, _currentTokens + periods * _maxTokens);
            _lastRefill = now;
        }
    }
}

/// <summary>
/// Exception thrown when the rate limit is exceeded.
/// </summary>
public sealed class RateLimitExceededException(string message) : Exception(message);
