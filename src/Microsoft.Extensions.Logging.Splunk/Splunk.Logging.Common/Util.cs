using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Splunk.Logging
{
    public static class Util
    {
        /// <summary>
        /// Get an IPAddress corresponding to the given hostname. If no address is available,
        /// throws an Exception. If more than one is available, returns only one of them.
        /// </summary>
        /// <param name="hostname">a hostname or IP address to turn into an IPAddress instance</param>
        /// <returns>an IPAddress instance</returns>
        public static IPAddress HostnameToIPAddress(this string hostname)
        {
            // If we can parse and IP address from hostname, use that.
            IPAddress addr;
            if (IPAddress.TryParse(hostname, out addr))
            {
                return addr;
            }

            // Otherwise, use DNS lookup to get at least one IP address.
            Task<IPHostEntry> taskGetHostEntryAsync = Dns.GetHostEntryAsync(hostname);
            if (taskGetHostEntryAsync?.Result?.AddressList == null || !taskGetHostEntryAsync.Result.AddressList.Any())
            {
                throw new Exception(string.Format("No IP address corresponding to hostname {0} found.", hostname));
            }
            return taskGetHostEntryAsync.Result.AddressList[0];
        }

        public static Socket OpenUdpSocket(IPAddress host, int port)
        {
            var socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            socket.Connect(host, port);
            return socket;
        }
    }
}
