using Client = System.Net.Sockets.UdpClient;

namespace Shared
{
    public class UdpBase
    {
        protected const int Port = 8888;
        protected const int TimeoutInMs = 100;

        protected Client client;
    }
}
