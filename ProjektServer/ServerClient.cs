using ClassLibrary;
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
        Server server;

        public ServerClient(TcpClient c, Server s)
        {
            client = c;
            server = s;
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
            object obj = Serializer.DeserializeObject(buffer);
            CheckRecievedObject(obj);

            //try
            //{
            //    await client.GetStream().WriteAsync(buffer, 0, buffer.Length);
            //}
            //catch (Exception e)
            //{
            //    server.RecieveMessage(null, "Förbindelse misslyckades i serverclient: " + e.Message);
            //}

            if (client.Connected)
                RecieveMessageAsync();
        }
        void CheckRecievedObject(object obj)
        {
            if(obj is MeObj)
            {
                server.RecieveMessage((MeObj)obj);
            }
        }

        public void SendMessageToClient(MeObj message)
        {
            byte[] buffer = Serializer.SerializeObject(message);
            try
            {
                NetworkStream stream = client.GetStream();
                stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                
            }
        }
    }
}
