﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary;

namespace ProjektClient
{
    //CLIENT
    public partial class Form1 : Form
    {
        TcpClient client;
        int port = 12345;
        public Form1()
        {
            InitializeComponent();
            client = new TcpClient();
            btnSend.Enabled = false;
            ClientRecieve cRecieve = new ClientRecieve(client, this);
            cRecieve.StartRecieving();
        }
        
        private delegate void SafeCallDelegate(string text);
        public void WriteMessage(string text)
        {
            if (lbxRecieved.InvokeRequired)
            {
                var d = new SafeCallDelegate(WriteMessage);
                lbxRecieved.Invoke(d, new object[] { text });
            } else
            {
                lbxRecieved.Items.Add(text);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!client.Connected) Connect();
        }

        private async void Connect()
        {
            try
            {
                IPAddress ip = IPAddress.Parse(txtIP.Text);
                await client.ConnectAsync(ip, port);
                
            }
            catch (Exception e)
            {
                lbxRecieved.Items.Add("Fel i connect" + e.Message);
            }
            if (client.Connected)
            {
                // Fungerar inte. Server mottar null
                SendMessageAsync(new ConnectionControl("asdlökasldk"));

                // Fungerar. Server mottar objektet
                SendMessageAsync(new ChatMessage("Me", "user"));

                btnConnect.Enabled = false;
                btnSend.Enabled = true;
            } else
            {
                
            }
        }

        private async void SendMessageAsync(object message)
        {
            byte[] buffer = Serializer.SerializeObject(message);
            try
            {
                NetworkStream stream = client.GetStream();
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                lbxRecieved.Items.Add("Fel med Send" + e.Message);
            }
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtMessage.Text == "")
                return;
            SendMessageAsync(new ChatMessage(txtMessage.Text, txtName.Text));
            txtMessage.Text = "";
        }
    }

}
