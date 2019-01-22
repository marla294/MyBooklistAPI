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
        public void TestCreateNewBookBlankTitle()
        {
            string newBookId = BookFactory.CreateNewBook(Db, "", "test author");

            if (newBookId == null)
            {
                Assert.Pass();
            }
            else
            {
                Int32.TryParse(newBookId, out int id);
                BookFactory.DeleteBook(Db, id);
                Assert.Fail();
            }
        }

        [Test]
        public void TestCreateNewBookBlankAuthor()
        {
            string newBookId = BookFactory.CreateNewBook(Db, "test book", "");

            if (newBookId == null)
            {
                Assert.Pass();
            }
            else
            {
                Int32.TryParse(newBookId, out int id);
                BookFactory.DeleteBook(Db, id);
                Assert.Fail();
            }
        }

        [Test]
        public void TestCreateNewBookTooLongTitle()
        {
            if (Int32.TryParse(BookFactory.CreateNewBook(Db, "asdf123456asdf123456asdf123456asdf", "test author"), out int id))
            {
                var testBook = BookFactory.LoadSingle(Db, id);

                Assert.IsNotNull(testBook);
                Assert.AreEqual("asdf123456asdf123456asdf123456", testBook.Title);

                BookFactory.DeleteBook(Db, id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestCreateNewBookTooLongAuthor()
        {
            if (Int32.TryParse(BookFactory.CreateNewBook(Db, "test book", "asdfgasdfgasdfgasdfgasdfgasdfgasdfgasdfgasdfgasdfgasdfgasdfg"), out int id))
            {
                var testBook = BookFactory.LoadSingle(Db, id);

                Assert.IsNotNull(testBook);
                Assert.AreEqual("asdfgasdfgasdfgasdfgasdfgasdfg", testBook.Author);

                BookFactory.DeleteBook(Db, id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestCreateNewBookInvalidCharacterTitle()
        {
            var book = BookFactory.CreateNewBook(Db, "*&^%asdf", "test author");

            if (book != null)
            {
                if (Int32.TryParse(book, out int id))
                {
                    BookFactory.DeleteBook(Db, id);
                }
                Assert.Fail();
            }
            else
            {
                Assert.Pass();
            }
        }

        [Test]
        public void TestCreateNewBookInvalidCharacterAuthor()
        {
            var book = BookFactory.CreateNewBook(Db, "test book", "!@#$%^&*1234");

            if (book != null)
            {
                if (Int32.TryParse(book, out int id))
                {
                    BookFactory.DeleteBook(Db, id);
                }
                Assert.Fail();
            }
            else
            {
                Assert.Pass();
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
