using System;
using System.Collections.Generic;
using Npgsql;

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

        public PostgreSQLConnection()
        {
            ConnectionString = "Host=127.0.0.1;Port=5433;Username=postgres;" +
                "Password=Password1;Database=booklist";
        }

        public void Insert(string table, params ColumnValuePairing[] insertValues)
        {
            var columns = $"{insertValues[0].Column}";
            var parameters = "@parameter1";

            List<object> values = new List<object>
            {
                insertValues[0].Value
            };

            for (var i = 1; i < insertValues.Length; i++)
            {
                columns = columns + $", {insertValues[i].Column}";
                values.Add(insertValues[i].Value);
                parameters = parameters + $", @parameter{(i+1).ToString()}";
            }

            var sql = $"insert into {table} ({columns}) values ({parameters})";

            ExecuteNonQuery(sql, values.ToArray());
        }

        public List<List<string>> Select(string[] columns, string table, string orderBy = "", string orderByDirection = "desc", int limit = -1)
        {
            var sql = $"select {columns[0]}";

            for (var i = 1; i < columns.Length; i++)
            {
                sql = sql + $", {columns[i]}";
            }

            sql = sql + $" from {table}";

            if (orderBy != "")
            {
                sql = sql + $" order by {orderBy} {orderByDirection}";
            }

            if (limit != -1)
            {
                sql = sql + $" limit {limit.ToString()}";
            }

            return ExecuteQuery(sql);
        }

        public void Update(string table, string setColumn, string setValue, string andOr,
                           params ColumnValuePairing[] whereValues) 
        {
            var sql = $"update {table} set {setColumn} = @parameter{(whereValues.Length + 1).ToString()} where " +
                $"{whereValues[0].Column} = @parameter1";

            UpdateAndDelete(sql, andOr, whereValues, setValue);
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
                    var addition = $"and {whereValues[i].Column} = @parameter{(i+1).ToString()}";
                    sql = sql + addition;
                }
            }

            return sql;
        }

        // pass in sql string with @parameter1 - @parameterN
        private void ExecuteNonQuery(string sql, params object[] parameters) 
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand(sql, connection))
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

                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public List<List<string>> ExecuteQuery(string sql)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                var results = ConnectionUtils.CreateEmptyResultSet(0);

                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    results = ReadDBResults(cmd.ExecuteReader());
                }

                connection.Close();

                return results;
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
