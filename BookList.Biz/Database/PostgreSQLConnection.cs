using System.Collections.Generic;
using Npgsql;

namespace BookList.Biz.Database
{
    public class WhereValues
    {
        public string Column { get; set; }
        public string Value { get; set; }

        public WhereValues(string col, string val) {
            Column = col;
            Value = val;
        }
    }

    public class PostgreSQLConnection : IDbConnection
    {
        private string ConnectionString { get; set; }

        public PostgreSQLConnection()
        {
            ConnectionString = "Host=127.0.0.1;Port=5433;Username=postgres;" +
                "Password=Password1;Database=booklist";
        }

        public List<List<string>> ExecuteCommand(string command)
        {
            NpgsqlConnection connection = 
                new NpgsqlConnection(ConnectionString);

            connection.Open();

            var cmd = new NpgsqlCommand(command, connection);
            var results = ReadDBResults(cmd.ExecuteReader());

            connection.Close();

            return results;
        }

        public void Update(string table, string setColumn, string setValue, string andOr,
                           params WhereValues[] whereValues) 
        {
            var sql = $"update {table} set {setColumn} = @parameter1 where " +
                $"{whereValues[0].Column} = {whereValues[0].Value}";

            ExecuteWithParameters(sql, setValue);
        }

        public void Delete(string table, params WhereValues[] whereValues)
        {
            var sql = $"delete from {table} where {whereValues[0].Column} = {whereValues[0].Value}";

            ExecuteCommand(sql);
        }

        // pass in sql string with @parameter1 - @parameterN
        private void ExecuteWithParameters(string sql, params string[] parameters) 
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    for (var i = 0; i < parameters.Length; i++) 
                    {
                        cmd.Parameters.AddWithValue($"@parameter{(i + 1).ToString()}", parameters[i].Replace("'", "''"));
                    }

                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        private List<List<string>> ReadDBResults(NpgsqlDataReader dataReader)
        {
            List<List<string>> results = 
                ConnectionUtils.CreateEmptyResultSet(dataReader.FieldCount);

            while (dataReader.Read())
            {
                for (var col = 0; col < dataReader.FieldCount; col++)
                {
                    results[col].Add(dataReader[col].ToString());
                }
            }

            return results;
        }
    }
}
