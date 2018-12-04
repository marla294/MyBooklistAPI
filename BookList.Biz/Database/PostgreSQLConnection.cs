using System;
using System.Collections.Generic;
using Npgsql;
using System.Linq;

namespace BookList.Biz.Database
{
    public class ColumnValuePairing
    {
        public string Column { get; set; }
        public object Value { get; set; }

        public ColumnValuePairing(string col, object val)
        {
            Column = col;
            Value = val;
        }
    }

    public class PostgreSQLConnection : IDbConnection
    {
        private string ConnectionString { get; set; }

        public List<List<string>> ResultSet { get; private set; }
        public string Table { get; private set; }
        public Dictionary<string, int> Columns { get; private set; } // All the columns on the table
        private string SQL { get; set; }
        private Dictionary<int, object> Parameters { get; set; }

        public PostgreSQLConnection()
        {
            ConnectionString = "Host=127.0.0.1;Port=5433;Username=postgres; Password=Password1;Database=booklist";
            ResetResults();
        }

        private void ResetResults()
        {
            ResultSet = ConnectionUtils.CreateEmptyResultSet(0);
            Table = "";
            Columns = new Dictionary<string, int>();
            SQL = "";
            Parameters = new Dictionary<int, object>();
        }

        private void SetTableAndColumns(string table)
        {
            Table = table;

            var colResults = ExecuteQuery($"select column_name from information_schema.columns where table_name = '{table}'");

            for (var i = 0; i < colResults[0].Count; i++)
            {
                Columns.Add(colResults[0][i], i);
            }
        }

        public PostgreSQLConnection Take(string table)
        {
            ResetResults();
            SetTableAndColumns(table);

            var sql = $"select * from {table}";

            SQL = sql;

            return this;
        }

        public PostgreSQLConnection Where(params ColumnValuePairing[] whereValues)
        {
            var parameterStart = Parameters.Count;
            var sql = SQL + $" where {whereValues[0].Column} = @parameter{(parameterStart + 1).ToString()}";
            Parameters.Add(parameterStart, whereValues[0].Value);

            for (var i = 1; i < whereValues.Length; i++)
            {
                var whereValue = whereValues[i];
                string snippet = $" and {whereValue.Column} = @parameter{(parameterStart + i + 1).ToString()}";
                sql = sql + snippet;
                Parameters.Add(parameterStart + i, whereValue.Value);
            }

            SQL = sql;

            return this;
        }

        public PostgreSQLConnection OrderBy(string orderBy, string orderByDirection = "desc")
        {
            SQL = SQL + $" order by {orderBy} {orderByDirection}";

            return this;
        }

        public PostgreSQLConnection Limit(int limit)
        {
            SQL = SQL + $" limit {limit}";

            return this;
        }

        public void Insert(string table, ColumnValuePairing[] insertValues)
        {
            var columns = $"{insertValues[0].Column}";
            var values = new List<object> { insertValues[0].Value };
            var parameters = "@parameter1";

            for (var i = 1; i < insertValues.Length; i++)
            {
                columns = columns + $", {insertValues[i].Column}";
                values.Add(insertValues[i].Value);
                parameters = parameters + $", @parameter{(i+1).ToString()}";
            }

            var sql = $"insert into {table} ({columns}) values ({parameters})";

            ExecuteNonQuery(sql, values.ToArray());
        }

        public void Update(string table, ColumnValuePairing setValue, string andOr,
                           params ColumnValuePairing[] whereValues) 
        {
            var sql = $"update {table} set {setValue.Column} = @parameter{(whereValues.Length + 1).ToString()} where " +
                $"{whereValues[0].Column} = @parameter1";

            UpdateAndDelete(sql, andOr, whereValues, setValue.Value.ToString());
        }

        public void Delete(string table, string andOr, params ColumnValuePairing[] whereValues)
        {
            var sql = $"delete from {table} where {whereValues[0].Column} = @parameter1";

            UpdateAndDelete(sql, andOr, whereValues);
        }

        private void UpdateAndDelete(string sql, string andOr, ColumnValuePairing[] whereValues, string setValue = null)
        {
            sql = AdditionalWhereValues(sql, andOr, whereValues);

            ExecuteNonQuery(sql, GetValuesArray(whereValues, setValue));
        }

        private object[] GetValuesArray(ColumnValuePairing[] columnValuePairings, params string[] extraValues)
        {
            var onlyValues = new List<object>();

            foreach (var pair in columnValuePairings)
            {
                onlyValues.Add(pair.Value);
            }

            foreach (var value in extraValues)
            {
                onlyValues.Add(value);
            }

            return onlyValues.ToArray();
        }

        private string AdditionalWhereValues(string sql, string andOr, params ColumnValuePairing[] whereValues)
        {
            if (whereValues.Length > 1)
            {
                for (var i = 1; i < whereValues.Length; i++)
                {
                    var addition = $"and {whereValues[i].Column} = @parameter{(i + 1).ToString()}";
                    sql = sql + addition;
                }
            }

            return sql;
        }

        public List<List<string>> Execute()
        {
            return ExecuteQuery(SQL, Parameters.Values.ToArray<object>());
        }

        // pass in sql string with @parameter1 - @parameterN
        private void ExecuteNonQuery(string sql, params object[] parameters) 
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                using (var cmd = AddParameters(new NpgsqlCommand(sql, connection), parameters))
                {
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        // pass in sql string with @parameter1 - @parameterN
        private List<List<string>> ExecuteQuery(string sql, params object[] parameters)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                var results = ConnectionUtils.CreateEmptyResultSet(0);

                using (var cmd = AddParameters(new NpgsqlCommand(sql, connection), parameters))
                {
                    results = ReadDBResults(cmd.ExecuteReader());
                }

                connection.Close();

                return results;
            }
        }

        private NpgsqlCommand AddParameters(NpgsqlCommand cmd, params object[] parameters)
        {
            for (var i = 0; i < parameters.Length; i++)
            {
                var param = $"@parameter{(i + 1).ToString()}";

                if (parameters[i] is int)
                {
                    cmd.Parameters.AddWithValue(param, (int)parameters[i]);
                }
                else if (parameters[i] is string)
                {
                    cmd.Parameters.AddWithValue(param, (string)parameters[i]);
                }
                else if (parameters[i] is bool)
                {
                    cmd.Parameters.AddWithValue(param, (bool)parameters[i]);
                }
                else if (parameters[i] is null)
                {
                    cmd.Parameters.AddWithValue(param, DBNull.Value);
                }
            }

            return cmd;
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
