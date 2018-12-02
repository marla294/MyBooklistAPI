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

        // Will select all results from the given table into the ResultSet property
        // Selects all columns
        public PostgreSQLConnection Take(string table)
        {
            SetTableAndColumns(table);

            var sql = $"select * from {table}";

            ResultSet = ExecuteQuery(sql);

            return this;
        }

        public PostgreSQLConnection Where(params ColumnValuePairing[] whereValues)
        {
            var results = ConnectionUtils.CreateEmptyResultSet(Columns.Count);

            for (var rowId = 0; rowId < ResultSet[0].Count; rowId++)
            {
                if (CanAddRow(rowId, whereValues))
                {
                    foreach(var col in Columns)
                    {
                        var colId = col.Value;
                        results[colId].Add(ResultSet[colId][rowId]);
                    }

                    ResultSet = results;
                }
            }

            return this;
        }

        private bool CanAddRow(int rowId, ColumnValuePairing[] whereValues)
        {
            var whereColumns = ColumnList(whereValues);

            var addVal = true;

            foreach (var whereColumn in whereColumns)
            {
                addVal &= ColumnCheck(rowId, whereColumn, whereValues);
            }

            return addVal;
        }

        private bool ColumnCheck(int rowId, string column, ColumnValuePairing[] whereValues)
        {
            var whereValue = whereValues.FirstOrDefault(val => val.Column == column);
            var columnId = Columns[column];
            return ResultSet[columnId][rowId] == (string)whereValue.Value;
        }

        private List<string> ColumnList(ColumnValuePairing[] pairings)
        {
            var columnList = new List<string>();

            foreach (var pairing in pairings)
            {
                columnList.Add(pairing.Column);
            }

            return columnList;
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
