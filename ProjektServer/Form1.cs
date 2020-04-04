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
            
            //try
            //{
            //    listener = new TcpListener(IPAddress.Any, port);
            //    listener.Start();
            //}
            //catch (Exception e)
            //{
            //    LogMessage("Fel med start" + e.Message);
            //}
            //RecieveClientAsync();
        }

        

        public object DeserializeObject(byte[] arrBytes)
        {
            if (arrBytes.Length == 0)
                return null;
            try
            {
                MemoryStream memStream = new MemoryStream();
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                MeObj obj = (MeObj)binForm.Deserialize(memStream);

                return obj;
            }
            catch (Exception e)
            {
                LogMessage("Fel med deserialisering: " + e.Message);
            }
            return null;
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
        public void WriteMessage(MeObj m)
        {
            LogMessage($"{m.UserName}: {m.TextMessage}");
        }
        
    }

    

}
