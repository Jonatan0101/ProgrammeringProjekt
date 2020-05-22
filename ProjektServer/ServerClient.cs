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
    // Objekt som server använder för att hantera klienter
    class ServerClient
    {
        TcpClient client;
        NetworkStream stream;
        Server server;
        public string UserName { get; set; }
        public IPEndPoint IPEnd { get; set; }

        public ServerClient(TcpClient c, Server s)
        {
            client = c;
            server = s;
            stream = c.GetStream();

            // Påbörjar mottagning av meddelanden asynkront
            RecieveMessageAsync();
        }
        // Tar emot meddelanden och skickar tillbaka det som kontroll att klienten är kopplad
        async void RecieveMessageAsync()
        {
            byte[] buffer = new byte[client.ReceiveBufferSize];
            IPEnd = (IPEndPoint)client.Client.RemoteEndPoint;
            int bytesRead = 0;

            try
            {
                stream = client.GetStream();
                bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            }catch (Exception){}

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
                server.RecieveMessage((ChatMessage)obj);
            } else if (obj is ConnectionControl)
            {
                
                ConnectionControl connection = (ConnectionControl)obj;
                server.RecieveMessage(null, connection.Status.ToString() + " status");
                if (connection.Status == ConnectionStatus.LogIn)
                {
                    UserName = connection.UserName;
                    server.AddUserToList(connection.UserName);
                    server.RecieveMessage(null, connection.Status.ToString() + " status");
                } else if(connection.Status == ConnectionStatus.LogOut)
                {
                    server.RemoveUserFromList(connection.UserName);
                }

            } else if (obj is null) {
                server.RecieveMessage(null, "Recieved null object");
            }
            else
            {
                server.RecieveMessage(null, "Recieved unknown");
            }
        }

        public async void SendMessageToClient(object message)
        {
            byte[] buffer = Serializer.SerializeObject(message);
            try
            {
                NetworkStream stream = client.GetStream();
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception){}
        }
    }
}
