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
    class Server
    {
        TcpListener listener;
        int port = 12345;

        public List<ServerClient> ConnectedClients { get; set; }
        Form1 form;
        public Server(Form1 f)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            RecieveClients();
            form = f;
        }
        void RecieveClients()
        {
            TcpClient client = listener.AcceptTcpClient();
            ConnectedClients.Add(new ServerClient(client, this));

            RecieveClients();
        }

        public void RecieveMessage(MeObj message, string logMessage = "")
        {
            if (logMessage == "")
                form.WriteMessage(message);
            else
                form.LogMessage(logMessage);
        }

    }
}
