﻿using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjektClient
{
    class ClientRecieve
    {
        TcpClient client;
        Form1 clientForm;
        public ClientRecieve(TcpClient tcpClient, Form1 form)
        {
            client = tcpClient;
            clientForm = form;
        }
        public void StartRecieving()
        {
            Thread t = new Thread(RecieveMessages);
            t.IsBackground = true;
            t.Start();
            clientForm.WriteMessage("Started Recieving");
        }
        // Tar emot meddelanden
        void RecieveMessages()
        {
            while (true)
            {
                byte[] buffer = new byte[client.ReceiveBufferSize];
                IPEndPoint clientEP = (IPEndPoint)client.Client.RemoteEndPoint;
                int bytesRead = 0;
                try
                {
                    NetworkStream stream = client.GetStream();
                    bytesRead = stream.Read(buffer, 0, buffer.Length);

                    object obj = Serializer.DeserializeObject(buffer);
                    CheckObject(obj);
                }
                catch (Exception e)
                {

                }
            }
        }
        // Kontrollerar inkommande objekt och utför relevanta handlingar
        void CheckObject(object obj)
        {
            if(obj is ChatMessage)
            {
                clientForm.WriteMessage($"{(obj as ChatMessage).UserName}: {(obj as ChatMessage).TextMessage}");
            } else if(obj is ConnectionControl)
            {
                if (((obj as ConnectionControl).ListOfUsers) != null)
                {
                    clientForm.UpdateUserList((obj as ConnectionControl).ListOfUsers);
                }
            }
        }
    }
}
