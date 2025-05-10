// ReSharper disable PublicConstructorInAbstractClass

using System.Threading.Channels;

namespace Helpers.Threading.Channels;

public class ChannelQueue<T>
{
    public Channel<T> Channel { get; }

    public ChannelQueue(int capacity = 100)
    {
        var options = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = false,
            SingleWriter = false
        };

        Channel =  System.Threading.Channels.Channel.CreateBounded<T>(options);
    }

    public ValueTask EnqueueAsync(T item, CancellationToken cancellationToken = default)
        => Channel.Writer.WriteAsync(item, cancellationToken);
}