namespace Helpers;

/// <summary>
/// Builds and executes a chain of middleware functions over a context of type <typeparamref name="T"/>.
/// Middlewares are invoked in the order they are added.
/// </summary>
/// <typeparam name="T">The context/data type flowing through the pipeline.</typeparam>
public sealed class PipelineBuilder<T>
{
    private readonly List<Func<T, Func<T, Task<T>>, Task<T>>> _middlewares = [];

    /// <summary>
    /// Adds a middleware to the pipeline.
    /// The middleware receives the current context and a delegate to invoke the next middleware.
    /// </summary>
    /// <param name="middleware">The middleware function.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public PipelineBuilder<T> Use(Func<T, Func<T, Task<T>>, Task<T>> middleware)
    {
        if (middleware == null) throw new ArgumentNullException(nameof(middleware));
        _middlewares.Add(middleware);
        return this;
    }

    /// <summary>
    /// Adds a simple transformation step (no access to next) as the terminal middleware.
    /// </summary>
    /// <param name="handler">A function that transforms the context.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public PipelineBuilder<T> UseTerminal(Func<T, Task<T>> handler)
    {
        if (handler == null) throw new ArgumentNullException(nameof(handler));
        _middlewares.Add((ctx, _) => handler(ctx));
        return this;
    }

    /// <summary>
    /// Builds and returns the composed pipeline delegate.
    /// </summary>
    /// <returns>A delegate that executes the full pipeline.</returns>
    public Func<T, Task<T>> Build()
    {
        Func<T, Task<T>> pipeline = ctx => Task.FromResult(ctx);

        for (int i = _middlewares.Count - 1; i >= 0; i--)
        {
            var current = _middlewares[i];
            var next = pipeline;
            pipeline = ctx => current(ctx, next);
        }

        return pipeline;
    }

    /// <summary>
    /// Builds the pipeline and immediately executes it with the given context.
    /// </summary>
    /// <param name="context">The initial context.</param>
    /// <returns>The resulting context after all middleware stages.</returns>
    public Task<T> ExecuteAsync(T context) => Build()(context);
}
