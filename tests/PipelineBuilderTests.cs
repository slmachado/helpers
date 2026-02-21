using FluentAssertions;
using Helpers;

namespace Helpers.Tests;

public class PipelineBuilderTests
{
    [Fact]
    public async Task ExecuteAsync_NoMiddleware_ReturnsContextUnchanged()
    {
        var pipeline = new PipelineBuilder<string>();
        var result = await pipeline.ExecuteAsync("input");
        result.Should().Be("input");
    }

    [Fact]
    public async Task ExecuteAsync_SingleMiddleware_TransformsContext()
    {
        var pipeline = new PipelineBuilder<string>()
            .Use(async (ctx, next) => await next(ctx.ToUpper()));

        var result = await pipeline.ExecuteAsync("hello");
        result.Should().Be("HELLO");
    }

    [Fact]
    public async Task ExecuteAsync_MultipleMiddlewares_ExecuteInOrder()
    {
        var log = new List<string>();

        var pipeline = new PipelineBuilder<string>()
            .Use(async (ctx, next) =>
            {
                log.Add("first:before");
                var r = await next(ctx);
                log.Add("first:after");
                return r;
            })
            .Use(async (ctx, next) =>
            {
                log.Add("second:before");
                var r = await next(ctx);
                log.Add("second:after");
                return r;
            });

        await pipeline.ExecuteAsync("data");

        log.Should().ContainInOrder(
            "first:before", "second:before", "second:after", "first:after");
    }

    [Fact]
    public async Task UseTerminal_TransformsContextWithoutCallingNext()
    {
        var pipeline = new PipelineBuilder<int>()
            .UseTerminal(ctx => Task.FromResult(ctx * 2));

        var result = await pipeline.ExecuteAsync(5);
        result.Should().Be(10);
    }

    [Fact]
    public async Task Build_CanBeReusedMultipleTimes()
    {
        var pipeline = new PipelineBuilder<int>()
            .Use(async (ctx, next) => await next(ctx + 1));

        var execute = pipeline.Build();

        var r1 = await execute(10);
        var r2 = await execute(20);

        r1.Should().Be(11);
        r2.Should().Be(21);
    }

    [Fact]
    public void Use_NullMiddleware_Throws()
    {
        var pipeline = new PipelineBuilder<string>();
        var act = () => pipeline.Use(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task ExecuteAsync_MiddlewareCanShortCircuit()
    {
        var secondCalled = false;

        var pipeline = new PipelineBuilder<string>()
            .Use((ctx, next) => Task.FromResult("short-circuit")) // does not call next
            .Use(async (ctx, next) => { secondCalled = true; return await next(ctx); });

        var result = await pipeline.ExecuteAsync("input");

        result.Should().Be("short-circuit");
        secondCalled.Should().BeFalse();
    }
}
