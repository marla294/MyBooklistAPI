using System.Web.Http;
using System.Web.Http.Cors;
using System.Collections.Generic;
using BookList.Biz.Models;
using BookList.Biz.Database;

public class ItemFromBody
{
    public int BookId { get; set; }
    public int ListId { get; set; }
}

namespace BookList.Service.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class BookListController : ApiController
    {
        PostgreSQLConnection Db { get; set; }

        public BookListController()
        {
            Db = new PostgreSQLConnection();
        }

        public List<BookListItem> Get()
        {
            return ItemFactory.LoadAll(Db);
        }

        // returns the id of the new list as a string
        public string Post([FromBody]ItemFromBody value)
        {
            return ItemFactory.CreateNewItem(Db, value.BookId, value.ListId);
        }

        public void Delete(int id)
        {
            ItemFactory.DeleteItem(Db, id);
        }
    }
}
