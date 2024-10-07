using System.Net;
using System.Net.Sockets;
using Xunit;

namespace Helpers.Tests
{
    public class NetworkHelperTests
    {
        /// <summary>
        /// Test GetLocalIpAddress to ensure it returns a valid IPv4 address.
        /// </summary>
        [Fact]
        public void GetLocalIpAddress_ShouldReturnValidIpAddress()
        {
            // Act
            var ipAddress = NetworkHelper.GetLocalIpAddress();

            // Assert
            Assert.NotNull(ipAddress);
            Assert.True(IPAddress.TryParse(ipAddress, out var parsedIp) && parsedIp.AddressFamily == AddressFamily.InterNetwork);
        }

        /// <summary>
        /// Test IsPortOpen to check if a port is open on the localhost.
        /// Note: For this test, ensure that the port 80 is open or change it to another port that is open.
        /// </summary>
        [Fact]
        public void IsPortOpen_ShouldReturnTrueForOpenPort()
        {
            // Arrange
            const string host = "localhost";
            int openPort;

            // Abrir um TcpListener em uma porta disponível
            using (var listener = new TcpListener(IPAddress.Loopback, 0))
            {
                listener.Start();
                openPort = ((IPEndPoint)listener.LocalEndpoint).Port;

                // Act
                var result = NetworkHelper.IsPortOpen(host, openPort, TimeSpan.FromMilliseconds(500));

                // Assert
                Assert.True(result);
                
                // Fechar o TcpListener
                listener.Stop();
            }
        }


        /// <summary>
        /// Test IsPortOpen to check if a closed port returns false.
        /// </summary>
        [Fact]
        public void IsPortOpen_ShouldReturnFalseForClosedPort()
        {
            // Arrange
            const string host = "localhost";
            const int closedPort = 65530; // Porta alta que provavelmente está fechada
            var timeout = TimeSpan.FromMilliseconds(500); // Timeout reduzido para resposta mais rápida

            // Act
            var result = NetworkHelper.IsPortOpen(host, closedPort, timeout);

            // Assert
            Assert.False(result);
        }


        /// <summary>
        /// Test PingHost to ensure it returns true for a reachable host.
        /// Note: This test assumes "google.com" is always reachable.
        /// </summary>
        [Fact]
        public void PingHost_ShouldReturnTrueForReachableHost()
        {
            // Arrange
            const string host = "google.com";

            // Act
            var result = NetworkHelper.PingHost(host);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test PingHost to ensure it returns false for an unreachable host.
        /// </summary>
        [Fact]
        public void PingHost_ShouldReturnFalseForUnreachableHost()
        {
            // Arrange
            const string host = "192.0.2.0"; // A guaranteed non-routable IP address

            // Act
            var result = NetworkHelper.PingHost(host);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Test GetMacAddress to ensure it returns a non-empty MAC address.
        /// </summary>
        [Fact]
        public void GetMacAddress_ShouldReturnNonEmptyMacAddress()
        {
            // Act
            var macAddress = NetworkHelper.GetMacAddress();

            // Assert
            Assert.False(string.IsNullOrEmpty(macAddress));
        }

        /// <summary>
        /// Test IsUrlReachableAsync to ensure it returns true for a reachable URL.
        /// </summary>
        [Fact]
        public async Task IsUrlReachableAsync_ShouldReturnTrueForReachableUrl()
        {
            // Arrange
            const string url = "https://www.google.com";

            // Act
            var result = await NetworkHelper.IsUrlReachableAsync(url);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Test IsUrlReachableAsync to ensure it returns false for an unreachable URL.
        /// </summary>
        [Fact]
        public async Task IsUrlReachableAsync_ShouldReturnFalseForUnreachableUrl()
        {
            // Arrange
            const string url = "https://nonexistent.domain";

            // Act
            var result = await NetworkHelper.IsUrlReachableAsync(url);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Test GetAvailablePort to ensure it returns a valid port number.
        /// </summary>
        [Fact]
        public void GetAvailablePort_ShouldReturnValidPort()
        {
            // Act
            var port = NetworkHelper.GetAvailablePort();

            // Assert
            Assert.InRange(port, 1, 65535);
        }
    }
}
