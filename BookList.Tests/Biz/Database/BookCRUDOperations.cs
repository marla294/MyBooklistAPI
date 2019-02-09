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
            int? newBookId = BookFactory.CreateNewBook(Db, "AsdfqwertyAsdfqwertyAsdfqwertyAsdfqwertyAsdfqwertyAsdfqwertyAsdfqwertyAsdfqwertyAsdfqwertyAsdfqwertyAsdfqwertyAsdfqwertyAsdfqwerty", "test author");

            if (newBookId == null)
            {
                Assert.Fail();
            }
            else
            {
                var testBook = BookFactory.LoadSingle(Db, (int)newBookId);

                Assert.IsNotNull(testBook);
                Assert.AreEqual("AsdfqwertyAsdfqwertyAsdfqwertyAsdfqwertyAsdfqwertyAsdfqwertyAsdfqwertyAsdfqwertyAsdfqwertyAsdfqwertyAsdfqwertyAsdfqwerty", testBook.Title);

                BookFactory.DeleteBook(Db, (int)newBookId);
            }
        }

        [Test]
        public void TestCreateNewBookTooLongAuthor()
        {
            int? newBookId = BookFactory.CreateNewBook(Db, "test book", "AsdfqwertyAsdfqwertyAsdfqwertyAsdfqwertyAsdfqwerty");

            if (newBookId == null)
            {
                Assert.Fail();
            }
            else
            {
                var testBook = BookFactory.LoadSingle(Db, (int)newBookId);

                Assert.IsNotNull(testBook);
                Assert.AreEqual("AsdfqwertyAsdfqwertyAsdfqwerty", testBook.Author);

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
                Assert.Fail("Book not created properly");
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
        public void TestUpdateBook()
        {
            var id = BookFactory.CreateNewBook(Db, "test book", "test author");

            if (id == null)
            {
                Assert.Fail("Book not created properly");
            }
            else
            {
                BookFactory.UpdateBook(Db, (int)id, "updated title", "updated author");

                var testBook = BookFactory.LoadSingle(Db, (int)id);

                Assert.IsNotNull(testBook);
                Assert.AreEqual("updated title", testBook.Title);
                Assert.AreEqual("updated author", testBook.Author);

                BookFactory.DeleteBook(Db, (int)id);
            }
        }

        [Test]
        public void TestUpdateBookBlankTitle()
        {
            int? id = BookFactory.CreateNewBook(Db, "test book", "test author");


            if (id == null)
            {
                Assert.Fail("Book not created properly");
            }
            else
            {
                BookFactory.UpdateBook(Db, (int)id, "", "updated author");

                var testBook = BookFactory.LoadSingle(Db, (int)id);

                Assert.IsNotNull(testBook);
                Assert.AreEqual("test book", testBook.Title);
                Assert.AreEqual("test author", testBook.Author);

                BookFactory.DeleteBook(Db, (int)id);
            }
        }

        [Test]
        public void TestUpdateBookBlankAuthor()
        {
            int? id = BookFactory.CreateNewBook(Db, "test book", "test author");


            if (id == null)
            {
                Assert.Fail("Book not created properly");
            }
            else
            {
                BookFactory.UpdateBook(Db, (int)id, "updated title", "");

                var testBook = BookFactory.LoadSingle(Db, (int)id);

                Assert.IsNotNull(testBook);
                Assert.AreEqual("test book", testBook.Title);
                Assert.AreEqual("test author", testBook.Author);

                BookFactory.DeleteBook(Db, (int)id);
            }
        }

        [Test]
        public void TestUpdateBookInvalidCharacterTitle()
        {
            int? id = BookFactory.CreateNewBook(Db, "test book", "test author");


            if (id == null)
            {
                Assert.Fail("Book not created properly");
            }
            else
            {
                BookFactory.UpdateBook(Db, (int)id, "*&^%asdf", "updated author");

                var testBook = BookFactory.LoadSingle(Db, (int)id);

                Assert.IsNotNull(testBook);
                Assert.AreEqual("test book", testBook.Title);
                Assert.AreEqual("test author", testBook.Author);

                BookFactory.DeleteBook(Db, (int)id);
            }
        }

        [Test]
        public void TestUpdateBookInvalidCharacterAuthor()
        {
            int? id = BookFactory.CreateNewBook(Db, "test book", "test author");


            if (id == null)
            {
                Assert.Fail("Book not created properly");
            }
            else
            {
                BookFactory.UpdateBook(Db, (int)id, "updated title", "*&^%asdf");

                var testBook = BookFactory.LoadSingle(Db, (int)id);

                Assert.IsNotNull(testBook);
                Assert.AreEqual("test book", testBook.Title);
                Assert.AreEqual("test author", testBook.Author);

                BookFactory.DeleteBook(Db, (int)id);
            }
        }

        [Test]
        public void TestDeleteBook()
        {
            int? newBookId = BookFactory.CreateNewBook(Db, "test book", "test author");

            if (newBookId == null)
            {
                Assert.Fail("Book not created properly");
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
