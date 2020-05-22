using System;
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
        ClientRecieve cRecieve;
        public Form1()
        {
            InitializeComponent();

            client = new TcpClient();
            btnSend.Enabled = false;
            cRecieve = new ClientRecieve(client, this);
            
        }
        
        private delegate void SafeCallDelegate(string text);
        private delegate void SafeCallDelegateList(List<string> list);
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
            if (client.Connected) cRecieve.StartRecieving();
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
                SendMessageAsync(new ConnectionControl(txtName.Text, ConnectionStatus.LogIn));

                btnConnect.Enabled = false;
                btnSend.Enabled = true;
            } else
            {
                
            }
        }

        internal void ShowMessageBox(string v)
        {
            MessageBox.Show(v);
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
        public void UpdateUserList(List<string> list)
        {
            lbxUsers.DataSource = null;
            foreach (var s in lbxUsers.Items)
            {
                lbxUsers.Items.Remove(s);
            }
            foreach (string s in list)
            {
                lbxUsers.Items.Add(s);
            }

            //if (lbxUsers.InvokeRequired)
            //{
            //    var d = new SafeCallDelegateList(UpdateUserList);
            //    lbxUsers.Invoke(d, new object[] { list });
            //} else
            //{
            //    lbxUsers.DataSource = list;
            //}
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtMessage.Text == "")
                return;
            SendMessageAsync(new ChatMessage(txtMessage.Text, txtName.Text));
            txtMessage.Text = "";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SendMessageAsync(new ConnectionControl(txtName.Text, ConnectionStatus.LogOut));
        }

        private void lbxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

}
