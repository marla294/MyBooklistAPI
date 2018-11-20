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

            sql = "select id from lists order by id desc limit 1";
            var id = ConnectionUtils.ExecuteCommand(new PostgreSQLConnection(), sql)[0][0];

            sql = "insert into booklist(book, username, done, rating, notes, " +
                $"sortorder, list) values (null, 1, false, null, '', 0, {id})";
            ConnectionUtils.ExecuteCommand(new PostgreSQLConnection(), sql);
        }
    }
}