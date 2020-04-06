using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjektServer
{
    class Server
    {
        TcpListener listener;
        int port = 12345;

        public List<ServerClient> ConnectedClients { get; set; } = new List<ServerClient>();
        Form1 form;
        public Server(Form1 f)
        {
            Thread t = new Thread(RecieveClients);
            t.IsBackground = true;
            t.Start();

            form = f;
        }
        void RecieveClients()
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                ConnectedClients.Add(new ServerClient(client, this));
            }
        }

        public void SendMessage(ChatMessage message)
        {
            foreach (ServerClient client in ConnectedClients)
            {
                client.SendMessageToClient(message);
            }
        }

        public void RecieveMessage(ChatMessage message, string logMessage = "")
        {
            if (logMessage == "")
            {
                form.WriteMessage(message);
                SendMessage(message);
            }else
                form.LogMessage(logMessage);
        }

        public void AddUserToList(string user)
        {
            form.AddUserToList(user + " -----");
        }

        public void AlertMessage(string message)
        {
            
        }

    }
}