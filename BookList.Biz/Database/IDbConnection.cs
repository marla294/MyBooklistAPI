using System.Collections.Generic;

namespace BookList.Biz.Database
{
    public interface IDbConnection
    {
        List<List<string>> ExecuteCommand(string command);
        List<List<string>> SelectAll(string table, string sortBy);
        void Update(string table, string setColumn, string setValue, string andOr,
                           params WhereValues[] whereValues);
        void Delete(string table, params WhereValues[] whereValues);
    }
}

