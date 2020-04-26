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
            // Sätter text på ett trådsäkert sätt
            if (lbxUsers.InvokeRequired)
            {
                var d = new SafeCallDelegate(AddUserToList);
                lbxUsers.Invoke(d, new object[] { user });
            }
            else
            {
                lbxUsers.Items.Add(user);
            }
        }
        void RemoveUser(string user)
        {
            if (lbxUsers.InvokeRequired)
            {
                var d = new SafeCallDelegate(RemoveUser);
                lbxUsers.Invoke(d, new object[] { user });
            } else
            {

            }
        }
        internal void RemoveUserFromList(string user)
        {
            if (lbxUsers.InvokeRequired)
            {
                var d = new SafeCallDelegate(RemoveUserFromList);
                lbxUsers.Invoke(d, new object[] { user });
            }
            else
            {
                for (int i = lbxUsers.Items.Count - 1; i > -1; i--)
                {
                    if (((string)lbxUsers.Items[i]).Contains(user))
                    {
                        lbxUsers.Items.RemoveAt(i);
                    }
                }
            }
            
        }

        private void lbxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lbxUsers_DoubleClick(object sender, EventArgs e)
        {
            if(lbxUsers.SelectedIndex != -1)
            {
                MessageBox.Show((string)lbxUsers.SelectedItem);
                server.RemoveClient(((string)lbxUsers.SelectedItem));
            }
        }
    }

    

}
