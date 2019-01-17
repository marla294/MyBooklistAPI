using System;
using NUnit.Framework;
using BookList.Biz.Database;

namespace BookList.Tests.Biz.Database
{
    [TestFixture]
    public class BookCRUDOperations
    {
        PostgreSQLConnection Db { get; set; }

        public BookCRUDOperations()
        {
            Db = new PostgreSQLConnection();
        }

        [Test]
        public void TestCreateNewBook()
        {
            if (Int32.TryParse(BookFactory.CreateNewBook(Db, "test book", "test author"), out int id))
            {
                var testBook = BookFactory.LoadSingle(Db, id);

                Assert.IsNotNull(testBook);
                Assert.AreEqual("test book", testBook.Title);
                Assert.AreEqual("test author", testBook.Author);

                BookFactory.DeleteBook(Db, id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestLoadAll()
        {
            if (Int32.TryParse(BookFactory.CreateNewBook(Db, "test book", "test author"), out int id))
            {
                var testBooks = BookFactory.LoadAll(Db);
                var testBook = testBooks.Find(book => book.Id == id);

                Assert.IsNotNull(testBooks);
                Assert.IsNotNull(testBook);

                BookFactory.DeleteBook(Db, id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestDeleteBook()
        {
            if (Int32.TryParse(BookFactory.CreateNewBook(Db, "test book", "test author"), out int id))
            {
                BookFactory.DeleteBook(Db, id);

                var testBook = BookFactory.LoadSingle(Db, id);

                Assert.IsNull(testBook);
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}
