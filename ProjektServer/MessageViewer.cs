using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjektServer
{
    // Använder databasemanager för att hämta en tabell som fylls i listbox
    public partial class MessageViewer : Form
    {
        DatabaseManager databaseManager;
        public MessageViewer(DatabaseManager dbm)
        {
            InitializeComponent();
            databaseManager = dbm;
        }

        private void MessageViewer_Load(object sender, EventArgs e)
        {
            UpdateMessageBox();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Är du säker?", "Varning", MessageBoxButtons.YesNo);
            if(dr == DialogResult.Yes)
            {
                if (lbxMessages.SelectedIndex != -1 && databaseManager.RemoveMessage((int)lbxMessages.SelectedValue))
                {
                } else {
                    MessageBox.Show("Fel. Meddelande ej borttaget");
                }
            }
            UpdateMessageBox();
        }

        void UpdateMessageBox()
        {
            lbxMessages.DataSource = null;
            lbxMessages.DisplayMember = "INFO";
            lbxMessages.ValueMember = "Id";
            lbxMessages.DataSource = databaseManager.GetTable("SELECT Id, (Username + ': ' +  Message) as INFO FROM Message");
        }

        // Fyller fält om specifikt meddelande
        private void lbxMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtValue.Text = lbxMessages.SelectedIndex.ToString();
            txtMessage.Text = databaseManager.GetCellText($"SELECT Message FROM Message WHERE Id = {lbxMessages.SelectedIndex}");
            txtUser.Text = databaseManager.GetCellText($"SELECT Username FROM Message WHERE Id = {lbxMessages.SelectedIndex}");
            txtTime.Text = databaseManager.GetCellText($"SELECT Time FROM Message WHERE Id = {lbxMessages.SelectedIndex}");
        }

        private void lbxMessages_SelectedValueChanged(object sender, EventArgs e)
        {

        }
    }
}
