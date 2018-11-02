using System.Web.Http;
using System.Collections.Generic;
using BookList.Biz.Models;
using BookList.Biz.Database;

namespace BookList.Service.Controllers
{
    public class Books : ApiController
    {
        public List<Book> Get()
        {
            return LoadBook.LoadAll();
        }
    }
}
