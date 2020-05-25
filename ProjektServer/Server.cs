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
        public DatabaseManager DbManager { get; set; }
        public List<ServerClient> ConnectedClients { get; set; } = new List<ServerClient>();
        Form1 form;
        public Server(Form1 f)
        {
            Thread t = new Thread(RecieveClients);
            t.IsBackground = true;
            t.Start();

            DbManager = new DatabaseManager();

            form = f;
        }
        // Tar emot klienter konstant med hjälp av en separat tråd som lägger till klienter i listan
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

        public void RemoveClient(string user)
        {
            for (int i = 0; i < ConnectedClients.Count; i++)
            {
                if (ConnectedClients[i].UserName == user)
                    ConnectedClients.Remove(ConnectedClients[i]);
            }
        }
        // Skickar objekt till alla anslutna
        public void SendMessage(object message)
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
                DbManager.InsertMessage(message.TextMessage, message.UserName);
            }
            else
                form.LogMessage(logMessage);
            UpdateConnectedClients();
        }

        public void AddUserToList(string user)
        {
            form.AddUserToList(user);
            UpdateConnectedClients();
        }
        public void RemoveUserFromList(string user)
        {
            form.RemoveUserFromList(user);
            UpdateConnectedClients();
        }

        public void UpdateConnectedClients()
        {
            ConnectionControl cc = new ConnectionControl();
            foreach (ServerClient sc in ConnectedClients)
            {
                cc.ListOfUsers.Add(sc.UserName);
            }
            SendMessage(cc);
            //form.ChangeTimer();
            
        }

        public void UpdateLastConnected()
        {
            if(ConnectedClients.Count != 0)
            {
                ConnectionControl cc = new ConnectionControl();
                foreach (ServerClient sc in ConnectedClients)
                {
                    cc.ListOfUsers.Add(sc.UserName);
                }
                ConnectedClients[ConnectedClients.Count - 1].SendMessageToClient(cc);
            }
                
            
        }
    }
}