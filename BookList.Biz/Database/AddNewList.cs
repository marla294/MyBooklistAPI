using System;
using System.Collections.Generic;
using BookList.Biz.Models;
using System.Linq;

namespace BookList.Biz.Database
{
    public static class AddNewList
    {
        public static void AddNew()
        {
            var sql = "insert into lists (name) values ('New List')";
            ConnectionUtils.ExecuteCommand(new PostgreSQLConnection(), sql);
        }
    }
}