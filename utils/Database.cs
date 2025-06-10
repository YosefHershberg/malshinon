
using MySql.Data.MySqlClient;

namespace Utils
{
    using TableType = List<Dictionary<string, object?>>;

    internal static class DBConnection
    {
        private static MySqlConnection? _connection;

        private static MySqlConnection Connect()
        {   
            string connStr = "server=localhost;user=root;password=SQLnov8ING;database=malshinon;";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            return conn;
        }

        public static void Disconnect()
        {
            _connection.Close();
        }

        public static MySqlCommand Command(string sql)
        {
            var cmd = new MySqlCommand { CommandText = sql };
            return cmd;
        }

        private static MySqlDataReader Send(MySqlCommand cmd)
        {
            if (_connection == null)
            {
                _connection = Connect();
            }
            cmd.Connection = _connection;
            return cmd.ExecuteReader();
        }

        private static TableType Parse(MySqlDataReader rdr)
        {
            var rows = new TableType();

            using (rdr)
            {
                while (rdr.Read())
                {
                    var row = new Dictionary<string, object?>(rdr.FieldCount);
                    for (int i = 0; i < rdr.FieldCount; i++)
                        row[rdr.GetName(i)] = rdr.IsDBNull(i) ? null : rdr.GetValue(i);

                    rows.Add(row);
                }
            }
            return rows;
        }

        public static TableType Execute(string sql)
        {
            MySqlCommand cmd = Command(sql);
            MySqlDataReader rdr = Send(cmd);
            return Parse(rdr);
        }

        public static void PrintResult(TableType keyValuePairs)
        {
            if (keyValuePairs.Count == 0)
            {
                Console.WriteLine("No results found.");
                return;
            }

            foreach (var row in keyValuePairs)
            {
                foreach (var kv in row)
                    Console.WriteLine($"{kv.Key}: {kv.Value}");
                Console.WriteLine("---");
            }
        }
    }
}
