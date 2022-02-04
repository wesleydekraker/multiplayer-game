using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Client = System.Net.Sockets.UdpClient;

namespace Shared
{
    public class UdpServer : UdpBase
    {
        private ConcurrentDictionary<IPEndPoint, (string Message, DateTime UpdatedAt)> clients;

        public UdpServer() : base()
        {
            this.client = new Client(Port);
            this.clients = new ConcurrentDictionary<IPEndPoint, (string, DateTime)>();

            _ = Task.Run(() => StartReceiving());
        }

        private async Task StartReceiving()
        {
            while (true)
            {
                try
                {
                    var udpReceivResult = await this.client.ReceiveAsync();
                    var message = Encoding.UTF8.GetString(udpReceivResult.Buffer);
                    this.clients[udpReceivResult.RemoteEndPoint] = (message, DateTime.Now);
                }
                catch (SocketException e)
                {
                    Debug.WriteLine(e);
                }
            }
        }

        public IEnumerable<T> GetReceivedMessages<T>()
        {
            return this.clients.Values.Select(client => JsonSerializer.Deserialize<T>(client.Message));
        }

        public void Send<T>(T message)
        {
            var jsonString = JsonSerializer.Serialize(message);
            var messageAsBytes = Encoding.UTF8.GetBytes(jsonString);

            foreach (var clientEndPoint in this.clients.Keys)
            {
                try
                {
                    this.client.Send(messageAsBytes, messageAsBytes.Length, clientEndPoint);
                }
                catch (SocketException e)
                {
                    Debug.WriteLine(e);
                }
            }
        }
    }
}
