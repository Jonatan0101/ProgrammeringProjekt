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

namespace ProjektServer
{
    //SERVER
    public partial class Form1 : Form
    {

        DatabaseManager dbManager;
        Server server;
        public Form1()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
            server = new Server(this);
        }

        
        private delegate void SafeCallDelegate(string text);
        public void LogMessage(string message)
        {
            // Sätter text på ett trådsäkert sätt
            if (listBox1.InvokeRequired)
            {
                var d = new SafeCallDelegate(LogMessage);
                listBox1.Invoke(d, new object[] { message });
            }
            else
            {
                listBox1.Items.Add(message);
            }
        }
        public void WriteMessage(ChatMessage m)
        {
            LogMessage($"{m.UserName}: {m.TextMessage}");
        }

        public void AddUserToList(string user)
        {
            lbxUsers.Items.Add(user);
        }
        
    }

    

}
