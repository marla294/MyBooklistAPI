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
            var values = $"{InsertValueForSQL(insertValues[0].Value)}";

            for (var i = 1; i < insertValues.Length; i++)
            {
                columns = columns + $", {insertValues[i].Column}";
                values = values + $", {InsertValueForSQL(insertValues[i].Value)}";
            }

            var sql = $"insert into {table} ({columns}) values ({values})";

            ExecuteCommand(sql);
        }

        private string InsertValueForSQL(object value)
        {
            if (value is string)
            {
                return $"'{value}'";
            }

            else if (value is null)
            {
                return "null";
            }

            else if (value is bool)
            {
                return (bool)value ? "true" : "false";
            }

            else if (value is int)
            {
                return $"{((int)value).ToString()}";
            }

            return "''";
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
