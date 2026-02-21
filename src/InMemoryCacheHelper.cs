namespace Helpers;

/// <summary>
/// A lightweight in-memory cache with per-entry TTL and thread-safety.
/// Does not depend on IMemoryCache — works without DI container.
/// </summary>
public sealed class InMemoryCacheHelper : IDisposable
{
    private readonly Dictionary<string, CacheEntry> _cache = new();
    private readonly object _lock = new();
    private readonly Timer _evictionTimer;

    /// <param name="evictionIntervalSeconds">How often expired entries are purged (default: 60s).</param>
    public InMemoryCacheHelper(int evictionIntervalSeconds = 60)
    {
        _evictionTimer = new Timer(
            _ => Evict(),
            null,
            TimeSpan.FromSeconds(evictionIntervalSeconds),
            TimeSpan.FromSeconds(evictionIntervalSeconds));
    }

    /// <summary>
    /// Returns the cached value if present and not expired; otherwise invokes the factory,
    /// stores the result, and returns it.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="factory">Function to produce the value on a cache miss.</param>
    /// <param name="ttl">Time-to-live for the entry. Defaults to 5 minutes.</param>
    public T GetOrSet<T>(string key, Func<T> factory, TimeSpan? ttl = null)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
        if (factory == null) throw new ArgumentNullException(nameof(factory));

        lock (_lock)
        {
            if (_cache.TryGetValue(key, out var entry) && !entry.IsExpired)
                return (T)entry.Value;
        }

        var value = factory();
        Set(key, value, ttl);
        return value;
    }

    /// <summary>
    /// Async version of <see cref="GetOrSet{T}"/>.
    /// </summary>
    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? ttl = null)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
        if (factory == null) throw new ArgumentNullException(nameof(factory));

        lock (_lock)
        {
            if (_cache.TryGetValue(key, out var entry) && !entry.IsExpired)
                return (T)entry.Value;
        }

        var value = await factory();
        Set(key, value, ttl);
        return value;
    }

    /// <summary>
    /// Stores a value in the cache with the given TTL.
    /// </summary>
    public void Set<T>(string key, T value, TimeSpan? ttl = null)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

        var expiry = DateTime.UtcNow.Add(ttl ?? TimeSpan.FromMinutes(5));
        lock (_lock)
        {
            _cache[key] = new CacheEntry(value!, expiry);
        }
    }

    /// <summary>
    /// Tries to get a cached value. Returns false if the key is missing or expired.
    /// </summary>
    public bool TryGet<T>(string key, out T? value)
    {
        value = default;
        if (string.IsNullOrWhiteSpace(key)) return false;

        lock (_lock)
        {
            if (_cache.TryGetValue(key, out var entry) && !entry.IsExpired)
            {
                value = (T)entry.Value;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Removes a specific entry from the cache.
    /// </summary>
    public void Invalidate(string key)
    {
        lock (_lock) _cache.Remove(key);
    }

    /// <summary>
    /// Removes all entries whose keys start with the given prefix.
    /// </summary>
    public void InvalidateByPrefix(string prefix)
    {
        lock (_lock)
        {
            var keys = _cache.Keys.Where(k => k.StartsWith(prefix)).ToList();
            foreach (var key in keys) _cache.Remove(key);
        }
    }

    /// <summary>Clears all entries from the cache.</summary>
    public void Clear()
    {
        lock (_lock) _cache.Clear();
    }

    /// <summary>Gets the number of non-expired entries currently in the cache.</summary>
    public int Count
    {
        get { lock (_lock) return _cache.Count(e => !e.Value.IsExpired); }
    }

    private void Evict()
    {
        lock (_lock)
        {
            var expired = _cache.Where(e => e.Value.IsExpired).Select(e => e.Key).ToList();
            foreach (var key in expired) _cache.Remove(key);
        }
    }

    public void Dispose() => _evictionTimer.Dispose();

    private sealed record CacheEntry(object Value, DateTime ExpiresAt)
    {
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    }
}
