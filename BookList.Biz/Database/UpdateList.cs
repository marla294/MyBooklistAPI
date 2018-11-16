using System;
using System.Collections.Generic;
using BookList.Biz.Models;
using System.Linq;

namespace BookList.Biz.Database
{
    public static class UpdateList
    {
        public static void UpdateListName(int id, string newName)
        {
            var sql = $"update lists set name = '{newName}' where id = {id.ToString()}";
            ConnectionUtils.ExecuteCommand(new PostgreSQLConnection(), sql);
        }
    }
}
