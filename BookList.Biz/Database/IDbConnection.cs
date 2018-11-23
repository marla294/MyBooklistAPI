using System.Collections.Generic;

namespace BookList.Biz.Database
{
    public interface IDbConnection
    {
        List<List<string>> ExecuteCommand(string command);
        void Update(string table, string setColumn, string setValue, 
                    string whereColumn, string whereValue);
    }
}

