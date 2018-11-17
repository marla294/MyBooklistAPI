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
            return LoadList.LoadAll();
        }

        public void Put(int id, [FromBody]ListName value)
        {
            UpdateList.UpdateListName(id, value.Name);
        }
    }
}
