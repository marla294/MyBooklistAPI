using System;
using System.Collections.Generic;
using BookList.Biz.Models;
using System.Linq;

namespace BookList.Biz.Database
{
    public static class LoadList
    {
        public static List<List> LoadAll()
        {
            return LoadByQuery("select * from lists order by id;");
        }

        static List<List> LoadByQuery(string sql)
        {
            var listResultSet = ConnectionUtils.ExecuteCommand(new PostgreSQLConnection(), sql);
            var listList = new List<List>();

            for (var i = 0; i < listResultSet[0].Count; i++)
            {
                List list = Int32.TryParse(listResultSet[0][i], out int id)
                    ? new List(id, listResultSet[1][i])
                    : new List();

                listList.Add(list);
            }

            return listList;
        }
    }
}
