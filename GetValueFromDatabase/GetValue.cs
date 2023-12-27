using Microsoft.Data.SqlClient;

namespace shopbackend.GetValueFromDatabase
{
    public class GetValue
    {
        public  string id;
        public string username;
        public string password;
        public GetValue() {
            string connectionString = "Server=ITIM\\SQLEXPRESS; Database=Admin; Trusted_Connection=True; TrustServerCertificate=True;";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string sqlQuery = "SELECT [Id]      ,[Username]      ,[Password]  FROM [Admin].[dbo].[Databases]";

            SqlCommand command = new SqlCommand(sqlQuery, connection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                id = reader["Id"].ToString();
                username = reader["Username"].ToString();
                password = reader["Password"].ToString();
            }
            connection.Close();
        }
    }
}
