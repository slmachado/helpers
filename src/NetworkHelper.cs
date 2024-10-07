using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Helpers;

public static class NetworkHelper
{
    /// <summary>
    /// Gets the primary local IPv4 address.
    /// </summary>
    /// <returns>The primary local IPv4 address as a string.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no network adapters with an IPv4 address are found.</exception>
    public static string GetLocalIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new InvalidOperationException("No network adapters with an IPv4 address in the system!");
    }

    /// <summary>
    /// Gets all local IPv4 addresses.
    /// </summary>
    /// <returns>A list of all local IPv4 addresses.</returns>
    public static IEnumerable<string> GetAllLocalIpAddresses()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        return host.AddressList
            .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
            .Select(ip => ip.ToString());
    }

    /// <summary>
    /// Checks if the specified port is open on the given host.
    /// </summary>
    /// <param name="host">The host name or IP address.</param>
    /// <param name="port">The port number.</param>
    /// <param name="timeout">The timeout in milliseconds for the port check (default is 1000ms).</param>
    /// <returns>True if the port is open; otherwise, false.</returns>
    public static bool IsPortOpen(string host, int port, TimeSpan timeout)
    {
        try
        {
            using var client = new TcpClient();
            var asyncResult = client.BeginConnect(host, port, null, null);
            var success = asyncResult.AsyncWaitHandle.WaitOne(timeout);
            if (success)
            {
                client.EndConnect(asyncResult); // Finalizar a conexão corretamente
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }


    /// <summary>
    /// Pings the specified host synchronously.
    /// </summary>
    /// <param name="host">The host name or IP address.</param>
    /// <returns>True if the ping is successful; otherwise, false.</returns>
    public static bool PingHost(string host)
    {
        try
        {
            using var pinger = new Ping();
            var reply = pinger.Send(host);
            return reply != null && reply.Status == IPStatus.Success;
        }
        catch (PingException)
        {
            return false;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Pings the specified host asynchronously.
    /// </summary>
    /// <param name="host">The host name or IP address.</param>
    /// <returns>A task that returns true if the ping is successful; otherwise, false.</returns>
    public static async Task<bool> PingHostAsync(string host)
    {
        try
        {
            using var pinger = new Ping();
            var reply = await pinger.SendPingAsync(host);
            return reply != null && reply.Status == IPStatus.Success;
        }
        catch (PingException)
        {
            return false;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the MAC address of the first network adapter.
    /// </summary>
    /// <returns>The MAC address as a string.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no network adapters are found.</exception>
    public static string GetMacAddress()
    {
        var nics = NetworkInterface.GetAllNetworkInterfaces();
        foreach (var adapter in nics)
        {
            if (adapter.NetworkInterfaceType != NetworkInterfaceType.Loopback && adapter.OperationalStatus == OperationalStatus.Up)
            {
                return adapter.GetPhysicalAddress().ToString();
            }
        }
        throw new InvalidOperationException("No network adapters found with an available MAC address.");
    }

    /// <summary>
    /// Gets all network interfaces on the local machine.
    /// </summary>
    /// <returns>A list of all network interfaces.</returns>
    public static IEnumerable<NetworkInterface> GetNetworkInterfaces()
    {
        return NetworkInterface.GetAllNetworkInterfaces();
    }

    /// <summary>
    /// Checks if the provided string is a valid IPv4 or IPv6 address.
    /// </summary>
    /// <param name="ipAddress">The IP address string to validate.</param>
    /// <returns>True if the IP address is valid; otherwise, false.</returns>
    public static bool IsValidIpAddress(string ipAddress)
    {
        return IPAddress.TryParse(ipAddress, out _);
    }

    /// <summary>
    /// Checks if the specified URL is reachable.
    /// </summary>
    /// <param name="url">The URL to check.</param>
    /// <returns>True if the URL is reachable; otherwise, false.</returns>
    public static async Task<bool> IsUrlReachableAsync(string url)
    {
        try
        {
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(3);
            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }


    /// <summary>
    /// Checks if the internet is available by pinging an external IP (e.g., Google's public DNS).
    /// </summary>
    /// <returns>True if the internet is available; otherwise, false.</returns>
    public static bool IsInternetAvailable()
    {
        return PingHost("8.8.8.8");
    }

    /// <summary>
    /// Gets an available port on the local machine.
    /// </summary>
    /// <returns>An available port number.</returns>
    public static int GetAvailablePort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        try
        {
            listener.Start();
            return ((IPEndPoint)listener.LocalEndpoint).Port;
        }
        finally
        {
            listener.Stop();
        }
    }
}
