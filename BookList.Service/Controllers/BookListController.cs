using System.Web.Http;
using System.Web.Http.Cors;
using System.Collections.Generic;
using BookList.Biz.Models;
using BookList.Biz.Database;

namespace BookList.Service.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class BookListController : ApiController
    {
        public List<BookListItem> Get()
        {
            return LoadItems.LoadAll();
        }
    }
}
