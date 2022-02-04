using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Client = System.Net.Sockets.UdpClient;

namespace Shared
{
    public class UdpClient : UdpBase
    {
        private bool startedReceiving = false;
        private (string Message, DateTime UpdatedAt) buffer;
        private IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, Port);

        public UdpClient() : base()
        {
            this.client = new Client
            {
                EnableBroadcast = true
            };
        }

        public T GetReceivedMessage<T>()
        {
            if ((DateTime.Now - this.buffer.UpdatedAt).TotalMilliseconds > TimeoutInMs)
                return default;
            else
                return JsonSerializer.Deserialize<T>(this.buffer.Message);
        }

        public void Send<T>(T message)
        {
            var jsonObject = JsonSerializer.Serialize(message);
            var messageAsBytes = Encoding.UTF8.GetBytes(jsonObject);

            try
            {
                this.client.Send(messageAsBytes, messageAsBytes.Length, this.endPoint);
            }
            catch (SocketException e)
            {
                Debug.WriteLine(e);
            }            

            if (!this.startedReceiving)
            {
                this.startedReceiving = true;
                _ = Task.Run(() => StartReceiving());
            }
        }

        private async Task StartReceiving()
        {
            while (true)
            {
                try
                {
                    var udpReceiveResult = await this.client.ReceiveAsync();
                    this.buffer = (Encoding.UTF8.GetString(udpReceiveResult.Buffer), DateTime.Now);
                }
                catch (SocketException e)
                {
                    Debug.WriteLine(e);
                }
            }
        }
    }
}
