using System.Collections.Generic;
using Npgsql;

namespace BookList.Biz.Database
{
    public class PostgreSQLConnection : IDbConnection
    {
        private string ConnectionString { get; set; }

        public PostgreSQLConnection()
        {
            ConnectionString = "Host=127.0.0.1;Port=5433;Username=postgres;Password=Password1;Database=booklist";
        }

        public List<List<string>> ExecuteCommand(string command)
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);

            connection.Open();

            var cmd = new NpgsqlCommand(command, connection);
            var results = ReadDBResults(cmd.ExecuteReader());

            connection.Close();

            return results;
        }

        public void UpdateListName(int id, string newName) {
            Update("lists", "name", newName, "id", id.ToString());
        }

        public void Update(string table, string setColumn, string setValue, string whereColumn, string whereValue) {
            var sql = $"update {table} set {setColumn} = @setValue where {whereColumn} = {whereValue}";

            NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
            connection.Open();

            using (var cmd = new NpgsqlCommand(sql, connection)) {
                cmd.Parameters.AddWithValue("@setValue", setValue.Replace("'", "''"));
                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }

        private List<List<string>> ReadDBResults(NpgsqlDataReader dataReader)
        {
            List<List<string>> results = ConnectionUtils.CreateEmptyResultSet(dataReader.FieldCount);

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
