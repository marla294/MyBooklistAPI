using System.Web.Http;
using System.Web.Http.Cors;
using System.Collections.Generic;
using BookList.Biz.Models;
using BookList.Biz.Database;

public class BookFromBody
{
    public string Title { get; set; }
    public string Author { get; set; }
}

namespace BookList.Service.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class BooksController : ApiController
    {
        PostgreSQLConnection Db { get; set; }

        public BooksController()
        {
            Db = new PostgreSQLConnection();
        }

        public List<Book> Get()
        {
            return BookFactory.LoadAll(Db);
        }

        // returns the id of the new list as a string
        public string Post([FromBody]BookFromBody value)
        {
            return BookFactory.CreateNewBook(Db, value.Title, value.Author);
        }

        public void Delete(int id)
        {
            BookFactory.DeleteBook(Db, id);
        }
    }
}
