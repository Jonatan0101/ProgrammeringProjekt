using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProjektClient
{
    class ClientSend
    {
        TcpClient client;
        Form1 form;
        int port = 12345;
        public ClientSend(TcpClient c, Form1 f)
        {
            client = c;
            form = f;
        }
        // Ansluter klient till server
        public async void Connect(string ip, string name)
        {
            try
            {
                IPAddress ipAd = IPAddress.Parse(ip);
                await client.ConnectAsync(ipAd, port);

            }
            catch (Exception e)
            {
                form.LogErrorMessage("Fel i connect ", e);
            }
            if (client.Connected)
            {
                SendMessageAsync(new ConnectionControl(name, ConnectionStatus.LogIn));

                form.SetConnectedButtons();
            }
        }

        // Skickar meddelande asynkront till server
        public async void SendMessageAsync(object message)
        {
            byte[] buffer = Serializer.SerializeObject(message);
            try
            {
                NetworkStream stream = client.GetStream();
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                form.LogErrorMessage("Fel med Send ", e);
            }
        }

    }
}
