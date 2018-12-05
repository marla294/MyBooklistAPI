using System.Collections.Generic;

namespace BookList.Biz.Database
{
    public interface IDbConnection
    {
        PostgreSQLConnection    Take(string table);
        PostgreSQLConnection    Where(ColumnValuePairing[] whereValues);
        PostgreSQLConnection    OrderBy(string orderBy, string orderByDirection = "desc");
        PostgreSQLConnection    Limit(int limit);
        PostgreSQLConnection    Insert(string table, ColumnValuePairing[] insertValues);
        void                    Update(string table, ColumnValuePairing setValue, string andOr, params ColumnValuePairing[] whereValues);
        void                    Delete(string table, string andOr, params ColumnValuePairing[] whereValues);
    }
}

