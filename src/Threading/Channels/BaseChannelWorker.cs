namespace Helpers.Threading.Channels;

// BaseChannelWorker.cs
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public abstract class BaseChannelWorker<T>(ChannelQueue<T> queue, IServiceScopeFactory scopeFactory, ILogger logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var item in queue.Channel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                await ProcessItemAsync(item, scope.ServiceProvider, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao processar item do tipo {Tipo}", typeof(T).Name);
            }
        }
    }

    protected abstract Task ProcessItemAsync(T item, IServiceProvider provider, CancellationToken token);
}