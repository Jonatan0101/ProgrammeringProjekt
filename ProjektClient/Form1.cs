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
            btnConnect.Enabled = false;
            btnSend.Enabled = true;
        }
        private async void SendMessageAsync(MeObj message)
        {
            byte[] buffer = SerializeToArray(message);
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

        byte[] SerializeToArray(MeObj message)
        {
            if (message == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, message);
                return ms.ToArray();
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtMessage.Text == "")
                return;
            SendMessageAsync(new MeObj(txtMessage.Text, txtName.Text));
            txtMessage.Text = "";
        }
    }

}
