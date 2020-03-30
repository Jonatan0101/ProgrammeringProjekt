using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProjektServer
{
    class ServerClient
    {
        TcpClient client;
        NetworkStream stream;


        public ServerClient(TcpClient c)
        {
            client = c;
            stream = c.GetStream();

            RecieveMessageAsync();
        }
        async void RecieveMessageAsync()
        {
            byte[] buffer = new byte[client.ReceiveBufferSize];
            IPEndPoint clientEP = (IPEndPoint)client.Client.RemoteEndPoint;
            int bytesRead = 0;

            try
            {
                NetworkStream stream = client.GetStream();
                bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

            }
            catch (Exception e)
            {

            }
        }
    }
}
