using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjektClient
{
    public partial class ConnectForm : Form
    {

        public IPAddress iPAddress { get; set; }
        public string UserName { get; set; }

        public ConnectForm()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if(IPAddress.TryParse(txtIP.Text, out IPAddress iP))
            {
                iPAddress = iP;
            } else
            {
                MessageBox.Show("Fel IP-Adress");
            }
            if(txtName.Text != "")
            {
                UserName = txtName.Text;
            } else
            {
                MessageBox.Show("Fyll i användarnamn");
            }
        }
    }
}
