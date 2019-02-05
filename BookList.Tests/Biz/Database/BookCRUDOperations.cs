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
            var id = BookFactory.CreateNewBook(Db, "test book", "test author");

            if (id == null)
            {
                Assert.Fail();
            }
            else
            {
                var testBook = BookFactory.LoadSingle(Db, (int)id);

                Assert.IsNotNull(testBook);
                Assert.AreEqual("test book", testBook.Title);
                Assert.AreEqual("test author", testBook.Author);

                BookFactory.DeleteBook(Db, (int)id);
            }
        }

        [Test]
        public void TestCreateNewBookBlankTitle()
        {
            int? newBookId = BookFactory.CreateNewBook(Db, "", "test author");

            if (newBookId == null)
            {
                Assert.Pass();
            }
            else
            {
                BookFactory.DeleteBook(Db, (int)newBookId);
                Assert.Fail();
            }
        }

        [Test]
        public void TestCreateNewBookBlankAuthor()
        {
            int? newBookId = BookFactory.CreateNewBook(Db, "test book", "");

            if (newBookId == null)
            {
                Assert.Pass();
            }
            else
            {
                BookFactory.DeleteBook(Db, (int)newBookId);
                Assert.Fail();
            }
        }

        [Test]
        public void TestCreateNewBookTooLongTitle()
        {
            int? newBookId = BookFactory.CreateNewBook(Db, "asdf123456asdf123456asdf123456asdf", "test author");

            if (newBookId == null)
            {
                Assert.Fail();
            }
            else
            {
                var testBook = BookFactory.LoadSingle(Db, (int)newBookId);

                Assert.IsNotNull(testBook);
                Assert.AreEqual("asdf123456asdf123456asdf123456", testBook.Title);

                BookFactory.DeleteBook(Db, (int)newBookId);
            }
        }

        [Test]
        public void TestCreateNewBookTooLongAuthor()
        {
            int? newBookId = BookFactory.CreateNewBook(Db, "test book", "asdfgasdfgasdfgasdfgasdfgasdfgasdfgasdfgasdfgasdfgasdfgasdfg");

            if (newBookId == null)
            {
                Assert.Fail();
            }
            else
            {
                var testBook = BookFactory.LoadSingle(Db, (int)newBookId);

                Assert.IsNotNull(testBook);
                Assert.AreEqual("asdfgasdfgasdfgasdfgasdfgasdfg", testBook.Author);

                BookFactory.DeleteBook(Db, (int)newBookId);
            }
        }

        [Test]
        public void TestCreateNewBookInvalidCharacterTitle()
        {
            int? newBookId = BookFactory.CreateNewBook(Db, "*&^%asdf", "test author");

            if (newBookId == null)
            {
                Assert.Pass();
            }
            else
            {
                BookFactory.DeleteBook(Db, (int)newBookId);
                Assert.Fail();
            }
        }

        [Test]
        public void TestCreateNewBookInvalidCharacterAuthor()
        {
            int? newBookId = BookFactory.CreateNewBook(Db, "test book", "!@#$%^&*1234");

            if (newBookId == null)
            {
                Assert.Pass();
            }
            else
            {
                BookFactory.DeleteBook(Db, (int)newBookId);
                Assert.Fail();
            }
        }

        [Test]
        public void TestLoadAll()
        {
            int? newBookId = BookFactory.CreateNewBook(Db, "test book", "test author");

            if (newBookId == null)
            {
                Assert.Fail();
            }
            else
            {
                var testBooks = BookFactory.LoadAll(Db);
                var testBook = testBooks.Find(book => book.Id == (int)newBookId);

                Assert.IsNotNull(testBooks);
                Assert.IsNotNull(testBook);

                BookFactory.DeleteBook(Db, (int)newBookId);
            }
        }

        [Test]
        public void TestDeleteBook()
        {
            int? newBookId = BookFactory.CreateNewBook(Db, "test book", "test author");

            if (newBookId == null)
            {
                Assert.Fail();
            }
            else
            {
                BookFactory.DeleteBook(Db, (int)newBookId);

                var testBook = BookFactory.LoadSingle(Db, (int)newBookId);

                Assert.IsNull(testBook);
            }
        }
    }
}
