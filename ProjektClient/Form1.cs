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
        ClientSend cSend;
        public Form1()
        {
            InitializeComponent();

            client = new TcpClient();
            btnSend.Enabled = false;
            cRecieve = new ClientRecieve(client, this);
            cSend = new ClientSend(client, this);
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
            if (!client.Connected) cSend.Connect(txtIP.Text, txtName.Text);
            if (client.Connected)
            {
                cRecieve.StartRecieving();
                txtName.ReadOnly = true;
                txtIP.ReadOnly = true;
            }

        }

        public void LogErrorMessage(string m, Exception e = null)
        {
            lbxRecieved.Items.Add(m + e.Message);
        }

        public void SetConnectedButtons()
        {
            btnConnect.Enabled = false;
            btnSend.Enabled = true;
        }

        // Uppdaterar anslutna klienter, ibland
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

        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtMessage.Text == "")
                return;
            //SendMessageAsync(new ChatMessage(txtMessage.Text, txtName.Text));
            cSend.SendMessageAsync(new ChatMessage(txtMessage.Text, txtName.Text));
            txtMessage.Text = "";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            cSend.SendMessageAsync(new ConnectionControl(txtName.Text, ConnectionStatus.LogOut));
        }

        private void lbxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

}
