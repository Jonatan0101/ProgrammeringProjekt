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
    class DatabaseManager
    {
        string connString;
        SqlConnection connection;

        public DatabaseManager()
        {
            connString = ConfigurationManager.ConnectionStrings["MessageDatabase"].ConnectionString;
        }

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

        public void InsertMessage(string message, string user)
        {
            string query = "INSERT INTO Message VALUES (@user, @message, @time)";

            using(connection = new SqlConnection(connString))
            using(SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@user", user);
                command.Parameters.AddWithValue("@message", message);
                command.Parameters.AddWithValue("@time", DateTime.Now);
                command.ExecuteNonQuery();
            }
        }

    }
}
