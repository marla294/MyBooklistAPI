using System;
using NUnit.Framework;
using BookList.Biz.Database;

namespace BookList.Tests.Biz.Database
{
    [TestFixture]
    public class BookCRUDOperations
    {
        [Test]
        public void TestCreateNewBook()
        {
            var db = new PostgreSQLConnection();

            if (Int32.TryParse(BookFactory.CreateNewBook(db, "test book", "test author"), out int id))
            {
                var testBook = BookFactory.LoadSingle(db, id);

                Assert.IsNotNull(testBook);
                Assert.AreEqual("test book", testBook.Title);
                Assert.AreEqual("test author", testBook.Author);

                BookFactory.DeleteBook(db, id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestLoadAll()
        {
            var db = new PostgreSQLConnection();

            if (Int32.TryParse(BookFactory.CreateNewBook(db, "test book", "test author"), out int id))
            {
                var testBooks = BookFactory.LoadAll(db);
                var testBook = testBooks.Find(book => book.Id == id);

                Assert.IsNotNull(testBooks);
                Assert.IsNotNull(testBook);

                BookFactory.DeleteBook(db, id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestDeleteBook()
        {
            var db = new PostgreSQLConnection();

            if (Int32.TryParse(BookFactory.CreateNewBook(db, "test book", "test author"), out int id))
            {
                BookFactory.DeleteBook(db, id);

                var testBook = BookFactory.LoadSingle(db, id);

                Assert.IsNull(testBook);
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}
