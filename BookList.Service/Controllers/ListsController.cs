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
        public List<List> Get()
        {
            return ListFactory.LoadAll();
        }

        public void Put(int id, [FromBody]ListName value)
        {
            ListFactory.UpdateListName(id, value.Name);
        }

        public void Post()
        {
            ListFactory.CreateNewList();
        }

        public void Delete(int id) 
        {
            ListFactory.DeleteList(id);
        }
    }
}
