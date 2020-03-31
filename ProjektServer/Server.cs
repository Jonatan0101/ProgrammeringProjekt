﻿using ClassLibrary;
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
            
            //RecieveClients();
            new Thread(RecieveClients).Start();
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

        public async void SendMessage(MeObj message)
        {
            foreach (ServerClient client in ConnectedClients)
            {
                await client.SendMessageToClient(message);
            }
        }

        public void RecieveMessage(MeObj message, string logMessage = "")
        {
            if (logMessage == "")
            {
                form.WriteMessage(message);
                SendMessage(message);
            }else
                form.LogMessage(logMessage);
        }

    }
}
