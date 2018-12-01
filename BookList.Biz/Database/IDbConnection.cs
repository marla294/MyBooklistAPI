using System.Collections.Generic;

namespace BookList.Biz.Database
{
    public interface IDbConnection
    {
        PostgreSQLConnection    Take(string table);
        PostgreSQLConnection    Where(ColumnValuePairing[] whereValues);
        void                    Insert(string table, ColumnValuePairing[] insertValues);
        List<List<string>>      Select(string[] columns, string table, string orderBy = "", string orderByDirection = "desc", int limit = -1);
        void                    Update(string table, ColumnValuePairing setValue, string andOr, params ColumnValuePairing[] whereValues);
        void                    Delete(string table, string andOr, params ColumnValuePairing[] whereValues);
    }
}

