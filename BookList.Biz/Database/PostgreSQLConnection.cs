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

    public class InsertValues
    {
        public string Column { get; set; }
        public object Value { get; set; }

        public InsertValues(string col, object val)
        {
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

        public void Insert(string table, params InsertValues[] insertValues)
        {
            var columns = $"{insertValues[0].Column}";

            var values = "";

            if (insertValues[0].Value is string)
            {
                values = $"'{insertValues[0].Value}'";
            }

            if (insertValues[0].Value is null)
            {
                values = "null";
            }

            if (insertValues[0].Value is bool)
            {
                values = (bool)insertValues[0].Value ? "true" : "false";
            }

            if (insertValues[0].Value is int)
            {
                values = $"{((int)insertValues[0].Value).ToString()}";
            }

            for (var i = 1; i < insertValues.Length; i++)
            {
                columns = columns + $", {insertValues[i].Column}";

                if (insertValues[i].Value is string)
                {
                    values = values + $", '{insertValues[i].Value}'";
                }

                if (insertValues[i].Value is null)
                {
                    values = values + ", null";
                }

                if (insertValues[i].Value is bool)
                {
                    values = (bool)insertValues[i].Value ? values + ", true" : values + ", false";
                }

                if (insertValues[i].Value is int)
                {
                    values = values + $", {((int)insertValues[i].Value).ToString()}";
                }
            }

            var sql = $"insert into {table} ({columns}) values ({values})";

            ExecuteCommand(sql);
        }

        public List<List<string>> SelectAll(string table, string sortBy)
        {
            var sql = $"select * from {table} order by {sortBy}";

            return ExecuteCommand(sql);
        }

        public void Update(string table, string setColumn, string setValue, string andOr,
                           params WhereValues[] whereValues) 
        {
            var sql = $"update {table} set {setColumn} = @parameter1 where " +
                $"{whereValues[0].Column} = {whereValues[0].Value}";

            sql = AdditionalWhereValues(sql, "and", whereValues);

            ExecuteWithParameters(sql, setValue);
        }

        public void Delete(string table, params WhereValues[] whereValues)
        {
            var sql = $"delete from {table} where {whereValues[0].Column} = {whereValues[0].Value}";

            sql = AdditionalWhereValues(sql, "and", whereValues);

            ExecuteCommand(sql);
        }

        private string AdditionalWhereValues(string sql, string andOr, params WhereValues[] whereValues)
        {
            if (whereValues.Length > 1)
            {
                for (var i = 1; i < whereValues.Length; i++)
                {
                    var addition = $"and {whereValues[i].Column} = {whereValues[i].Value}";
                    sql = sql + addition;
                }
            }

            return sql;
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
