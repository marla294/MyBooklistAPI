using System.Collections.Generic;

namespace BookList.Biz.Database
{
    public interface IDbConnection
    {
        List<List<string>> ExecuteQuery(string command);
        List<List<string>> Select(string[] columns, string table, string orderBy = "", string orderByDirection = "desc", int limit = -1);
        void Update(string table, string setColumn, string setValue, string andOr,
                           params WhereValues[] whereValues);
        void Delete(string table, params WhereValues[] whereValues);
        void Insert(string table, params InsertValues[] insertValues);
    }
}

