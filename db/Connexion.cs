using System.Data.Odbc;

namespace tsenaFinal.db
{
    public class Connexion
    {
        private OdbcConnection? conn; // Marqué comme nullable

        public Connexion()
        {
            conn = null; // Initialisation explicite
        }

        public void OpenConnection(string connectionString)
        {
            if (conn == null)
            {
                conn = new OdbcConnection(connectionString);
            }
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }
        }

        public void CloseConnection()
        {
            if (conn != null && conn.State != System.Data.ConnectionState.Closed)
            {
                conn.Close();
            }
        }

        public List<object[]> ExecuteQuery(string query)
        {
            var results = new List<object[]>();
            if (conn != null && conn.State == System.Data.ConnectionState.Open)
            {
                using (OdbcCommand command = new OdbcCommand(query, conn))
                {
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            results.Add(row);
                        }
                    }
                }
            }
            return results;
        }

        public void ExecuteUpdate(string query)
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Open)
            {
                using (OdbcCommand command = new OdbcCommand(query, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}