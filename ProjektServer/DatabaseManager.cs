using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ProjektServer
{
    // Hanterar databas, läser och skriver till sql server
    public class DatabaseManager
    {
        string connString;
        SqlConnection connection;

        // Läser in strängen till servern
        public DatabaseManager()
        {
            connString = ConfigurationManager.ConnectionStrings["MessageDatabase"].ConnectionString;
        }

        // Returnerar en tabell som kan användas i listboxes
        public DataTable GetTable(string query)
        {
            DataTable table;
            using(connection = new SqlConnection(connString))
            using(SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                table = new DataTable();
                adapter.Fill(table);
            }
            return table;
        }
        // Hämtar ett värde från en cell och returnerar som string, är det en datetime så konverteras den först
        public string GetCellText(string query)
        {
            string text;
            using (connection = new SqlConnection(connString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                try
                { 
                    text = (string)command.ExecuteScalar();
                    return text;
                }
                catch (InvalidCastException e)
                {
                    DateTime myDateTime = (DateTime)command.ExecuteScalar();
                    return myDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                
            }
            return "";
        }

        // Sparar ner ett meddelande
        public void InsertMessage(string message, string user)
        {
            string query = "INSERT INTO Message VALUES (@user, @message, @time)";

            using(connection = new SqlConnection(connString))
            using(SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                // Fyller i parametrar i query för att använda andra värden
                command.Parameters.AddWithValue("@user", user);
                command.Parameters.AddWithValue("@message", message);
                command.Parameters.AddWithValue("@time", DateTime.Now);
                command.ExecuteNonQuery();
            }
        }

        // Tar bort meddelande med hjälp av value från MessageVi
        public bool RemoveMessage(int i)
        {
            string query = "DELETE FROM Message WHERE Id = @value";

            using (connection = new SqlConnection(connString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                try
                {
                    command.Parameters.AddWithValue("@value", i);
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (NullReferenceException)
                {
                    return false;
                }
            }
        }

    }
}
