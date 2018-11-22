using System.Collections.Generic;

namespace BookList.Biz.Database
{
    public interface IDbConnection
    {
        List<List<string>> ExecuteCommand(string command);
        void UpdateListName(int id, string newName);
    }
}

