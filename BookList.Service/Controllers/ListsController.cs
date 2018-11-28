using System.Web.Http;
using System.Web.Http.Cors;
using System.Collections.Generic;
using BookList.Biz.Models;
using BookList.Biz.Database;

public class ListName {
    public string Name { get; set; }
}

namespace BookList.Service.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class ListsController : ApiController
    {
        PostgreSQLConnection Db { get; set; }

        public ListsController()
        {
            Db = new PostgreSQLConnection();
        }

        public List<List> Get()
        {
            return ListFactory.LoadAll(Db);
        }

        public void Put(int id, [FromBody]ListName value)
        {
            ListFactory.UpdateListName(Db, id, value.Name);
        }

        public void Post()
        {
            ListFactory.CreateNewList(Db);
        }

        public void Delete(int id) 
        {
            ListFactory.DeleteList(Db, id);
        }
    }
}
