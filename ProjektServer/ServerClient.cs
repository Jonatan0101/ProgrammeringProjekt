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
            catch (Exception){}

            object obj = Serializer.DeserializeObject(buffer);
            CheckRecievedObject(obj);

            try
            {
                buffer = Serializer.SerializeObject(new ConnectionControl());
                await client.GetStream().WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                server.RecieveMessage(null, "Förbindelse misslyckades i serverclient: " + e.Message);
            }

            if (client.Connected)
                RecieveMessageAsync();
        }
        void CheckRecievedObject(object obj)
        {
            if (obj is ChatMessage)
            {
                if ((obj as ChatMessage).UserName == "LogIn")
                    server.RecieveMessage(null, "Added person to chat");
                else
                    server.RecieveMessage((ChatMessage)obj);
            } else if (obj is ConnectionControl)
            {
                ConnectionControl cc = (ConnectionControl)obj;
                if (cc.UserName == "LogIn")
                {
                    server.AddUserToList(cc.UserName);
                }
            } else if (obj is null) {
                server.RecieveMessage(null, "Recieved null object");
            }
            else
            {
                server.RecieveMessage(null, "Recieved unknown object");
            }
        }

        public async void SendMessageToClient(ChatMessage message)
        {
            byte[] buffer = Serializer.SerializeObject(message);
            try
            {
                NetworkStream stream = client.GetStream();
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                
            }
        }
    }
}
