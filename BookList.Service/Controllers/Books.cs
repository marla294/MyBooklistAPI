using System.Web.Http;
using System.Web.Http.Cors;
using System.Collections.Generic;
using BookList.Biz.Models;
using BookList.Biz.Database;

namespace BookList.Service.Controllers
{
    [EnableCors(origins: "http://127.0.0.1:8080", headers: "*", methods: "*")]
    public class Books : ApiController
    {
        public List<Book> Get()
        {
            return LoadBook.LoadAll();
        }
    }
}
