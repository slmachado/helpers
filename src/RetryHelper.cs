namespace Helpers;

/// <summary>
/// Provides retry policies for executing actions that may fail transiently.
/// </summary>
public static class RetryHelper
{
    /// <summary>
    /// Executes a synchronous action with a fixed retry delay.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <param name="maxRetries">Maximum number of retry attempts.</param>
    /// <param name="delay">Delay between retries.</param>
    /// <param name="onRetry">Optional callback invoked on each failure with the exception and attempt number.</param>
    public static void Execute(
        Action action,
        int maxRetries = 3,
        TimeSpan? delay = null,
        Action<Exception, int>? onRetry = null)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));
        ValidateRetries(maxRetries);

        var retryDelay = delay ?? TimeSpan.FromMilliseconds(200);

        for (int attempt = 1; attempt <= maxRetries + 1; attempt++)
        {
            try
            {
                action();
                return;
            }
            catch (Exception ex) when (attempt <= maxRetries)
            {
                onRetry?.Invoke(ex, attempt);
                Thread.Sleep(retryDelay);
            }
        }
    }

    /// <summary>
    /// Executes a synchronous function with a fixed retry delay and returns its result.
    /// </summary>
    /// <typeparam name="T">The return type.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <param name="maxRetries">Maximum number of retry attempts.</param>
    /// <param name="delay">Delay between retries.</param>
    /// <param name="onRetry">Optional callback invoked on each failure with the exception and attempt number.</param>
    /// <returns>The result of the function.</returns>
    public static T Execute<T>(
        Func<T> func,
        int maxRetries = 3,
        TimeSpan? delay = null,
        Action<Exception, int>? onRetry = null)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));
        ValidateRetries(maxRetries);

        var retryDelay = delay ?? TimeSpan.FromMilliseconds(200);

        for (int attempt = 1; attempt <= maxRetries + 1; attempt++)
        {
            try
            {
                return func();
            }
            catch (Exception ex) when (attempt <= maxRetries)
            {
                onRetry?.Invoke(ex, attempt);
                Thread.Sleep(retryDelay);
            }
        }

        // This is unreachable but required by the compiler
        return func();
    }

    /// <summary>
    /// Executes an asynchronous function with a fixed retry delay.
    /// </summary>
    /// <param name="func">The async function to execute.</param>
    /// <param name="maxRetries">Maximum number of retry attempts.</param>
    /// <param name="delay">Delay between retries.</param>
    /// <param name="onRetry">Optional async callback invoked on each failure.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public static async Task ExecuteAsync(
        Func<Task> func,
        int maxRetries = 3,
        TimeSpan? delay = null,
        Func<Exception, int, Task>? onRetry = null,
        CancellationToken cancellationToken = default)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));
        ValidateRetries(maxRetries);

        var retryDelay = delay ?? TimeSpan.FromMilliseconds(200);

        for (int attempt = 1; attempt <= maxRetries + 1; attempt++)
        {
            try
            {
                await func();
                return;
            }
            catch (Exception ex) when (attempt <= maxRetries)
            {
                if (onRetry != null) await onRetry(ex, attempt);
                await Task.Delay(retryDelay, cancellationToken);
            }
        }
    }

    /// <summary>
    /// Executes an asynchronous function with a fixed retry delay and returns a result.
    /// </summary>
    /// <typeparam name="T">The return type.</typeparam>
    /// <param name="func">The async function to execute.</param>
    /// <param name="maxRetries">Maximum number of retry attempts.</param>
    /// <param name="delay">Delay between retries.</param>
    /// <param name="onRetry">Optional async callback invoked on each failure.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The result of the function.</returns>
    public static async Task<T> ExecuteAsync<T>(
        Func<Task<T>> func,
        int maxRetries = 3,
        TimeSpan? delay = null,
        Func<Exception, int, Task>? onRetry = null,
        CancellationToken cancellationToken = default)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));
        ValidateRetries(maxRetries);

        var retryDelay = delay ?? TimeSpan.FromMilliseconds(200);

        for (int attempt = 1; attempt <= maxRetries + 1; attempt++)
        {
            try
            {
                return await func();
            }
            catch (Exception ex) when (attempt <= maxRetries)
            {
                if (onRetry != null) await onRetry(ex, attempt);
                await Task.Delay(retryDelay, cancellationToken);
            }
        }

        return await func();
    }

    /// <summary>
    /// Executes an asynchronous function with exponential backoff between retries.
    /// </summary>
    /// <param name="func">The async function to execute.</param>
    /// <param name="maxRetries">Maximum number of retry attempts.</param>
    /// <param name="initialDelay">Initial delay (doubles on each attempt).</param>
    /// <param name="onRetry">Optional async callback invoked on each failure.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public static async Task ExecuteWithExponentialBackoffAsync(
        Func<Task> func,
        int maxRetries = 5,
        TimeSpan? initialDelay = null,
        Func<Exception, int, Task>? onRetry = null,
        CancellationToken cancellationToken = default)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));
        ValidateRetries(maxRetries);

        var delay = initialDelay ?? TimeSpan.FromMilliseconds(100);

        for (int attempt = 1; attempt <= maxRetries + 1; attempt++)
        {
            try
            {
                await func();
                return;
            }
            catch (Exception ex) when (attempt <= maxRetries)
            {
                if (onRetry != null) await onRetry(ex, attempt);
                await Task.Delay(delay, cancellationToken);
                delay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * 2);
            }
        }
    }

    private static void ValidateRetries(int maxRetries)
    {
        if (maxRetries < 1)
            throw new ArgumentOutOfRangeException(nameof(maxRetries), "maxRetries must be at least 1.");
    }
}
