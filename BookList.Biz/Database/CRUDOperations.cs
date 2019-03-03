using System;
using System.Collections.Generic;

namespace BookList.Biz.Database
{
    public class CRUDOperations
    {
        public int Create(IDbConnection db, string table, string col, object val)
        {
            db.Insert(table, new KeyValuePair<string, object>[] {
                                Pairing.Of(col, val)
            }).Execute();

            return Int32.TryParse(db.Take("lists").OrderBy("id", "desc").Limit(1).Execute()[0][0], out int id) ? id : 0;
        }
    }
}
