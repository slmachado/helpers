using FluentAssertions;
using Helpers;

namespace Helpers.Tests;

public class InMemoryCacheHelperTests : IDisposable
{
    private readonly InMemoryCacheHelper _cache = new();

    [Fact]
    public void GetOrSet_CacheMiss_InvokesFactory()
    {
        int factoryCallCount = 0;
        var result = _cache.GetOrSet("key1", () => { factoryCallCount++; return "value1"; });

        result.Should().Be("value1");
        factoryCallCount.Should().Be(1);
    }

    [Fact]
    public void GetOrSet_CacheHit_DoesNotInvokeFactory()
    {
        int factoryCallCount = 0;
        _cache.GetOrSet("key2", () => { factoryCallCount++; return "value2"; });
        _cache.GetOrSet("key2", () => { factoryCallCount++; return "other"; });

        factoryCallCount.Should().Be(1);
    }

    [Fact]
    public async Task GetOrSetAsync_CacheMiss_InvokesFactory()
    {
        int callCount = 0;
        var result = await _cache.GetOrSetAsync("async-key", async () =>
        {
            callCount++;
            await Task.Delay(1);
            return 99;
        });

        result.Should().Be(99);
        callCount.Should().Be(1);
    }

    [Fact]
    public void Set_And_TryGet_ReturnsValue()
    {
        _cache.Set("mykey", 123);
        var found = _cache.TryGet<int>("mykey", out var value);

        found.Should().BeTrue();
        value.Should().Be(123);
    }

    [Fact]
    public void TryGet_ExpiredEntry_ReturnsFalse()
    {
        _cache.Set("exp-key", "soon expired", TimeSpan.FromMilliseconds(50));
        Thread.Sleep(100);

        _cache.TryGet<string>("exp-key", out _).Should().BeFalse();
    }

    [Fact]
    public void TryGet_MissingKey_ReturnsFalse()
    {
        _cache.TryGet<string>("nonexistent", out _).Should().BeFalse();
    }

    [Fact]
    public void Invalidate_RemovesKey()
    {
        _cache.Set("del-key", "value");
        _cache.Invalidate("del-key");
        _cache.TryGet<string>("del-key", out _).Should().BeFalse();
    }

    [Fact]
    public void InvalidateByPrefix_RemovesMatchingKeys()
    {
        _cache.Set("user:1", "Alice");
        _cache.Set("user:2", "Bob");
        _cache.Set("product:1", "Pen");

        _cache.InvalidateByPrefix("user:");

        _cache.TryGet<string>("user:1", out _).Should().BeFalse();
        _cache.TryGet<string>("user:2", out _).Should().BeFalse();
        _cache.TryGet<string>("product:1", out _).Should().BeTrue();
    }

    [Fact]
    public void Clear_RemovesAllEntries()
    {
        _cache.Set("a", 1);
        _cache.Set("b", 2);
        _cache.Clear();
        _cache.Count.Should().Be(0);
    }

    [Fact]
    public void Count_ReturnsOnlyNonExpiredEntries()
    {
        _cache.Set("live", "ok", TimeSpan.FromMinutes(5));
        _cache.Set("dead", "gone", TimeSpan.FromMilliseconds(10));
        Thread.Sleep(50);

        _cache.Count.Should().Be(1);
    }

    public void Dispose() => _cache.Dispose();
}
