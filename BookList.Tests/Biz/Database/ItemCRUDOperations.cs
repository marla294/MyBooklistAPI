using System;
using NUnit.Framework;
using BookList.Biz.Database;

namespace BookList.Tests.Biz.Database
{
    [TestFixture]
    public class ItemCRUDOperations
    {
        [Test]
        public void TestCreateNewItem()
        {
            var db = new PostgreSQLConnection();

            Int32.TryParse(BookFactory.CreateNewBook(db, "test book", "test author"), out int bookId);
            Int32.TryParse(ListFactory.CreateNewList(db, "test list"), out int listId);

            if (Int32.TryParse(ItemFactory.CreateNewItem(db, bookId, listId), out int id))
            {
                var testItem = ItemFactory.LoadSingle(db, id);

                Assert.IsNotNull(testItem);
                Assert.AreEqual(bookId, testItem.Book.Id);
                Assert.AreEqual(listId, testItem.ListId);

                ItemFactory.DeleteItem(db, id);

                BookFactory.DeleteBook(db, bookId);
                ListFactory.DeleteList(db, listId);
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

            var testItemsList = ItemFactory.LoadAll(db);
            //var testItem = testItemsList.Find(item => item.Id == 1);

            Assert.IsNotNull(testItemsList);
            //Assert.IsNotNull(testItem);
            //Assert.AreEqual(2, testItem.Book.Id);
        }

        [Test]
        public void TestDeleteItem()
        {
            var db = new PostgreSQLConnection();

            Int32.TryParse(BookFactory.CreateNewBook(db, "test book", "test author"), out int bookId);
            Int32.TryParse(ListFactory.CreateNewList(db, "test list"), out int listId);

            if (Int32.TryParse(ItemFactory.CreateNewItem(db, bookId, listId), out int id))
            {
                ItemFactory.DeleteItem(db, id);

                var testItem = ItemFactory.LoadSingle(db, id);

                Assert.IsNull(testItem);

                BookFactory.DeleteBook(db, bookId);
                ListFactory.DeleteList(db, listId);
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}
