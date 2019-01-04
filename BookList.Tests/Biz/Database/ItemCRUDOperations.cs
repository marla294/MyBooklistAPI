using System;
using NUnit.Framework;
using BookList.Biz.Database;

namespace BookList.Tests.Biz.Database
{
    [TestFixture]
    public class ItemCRUDOperations
    {
        PostgreSQLConnection Db { get; set; }
        int BookId { get; set; }
        int ListId { get; set; }
        int UserId { get; set; }

        public ItemCRUDOperations()
        {
            Db = new PostgreSQLConnection();

            if (Int32.TryParse(UserFactory.CreateNewUser(Db, "testuser", "testuseritem", "password"), out int userId))
            {
                UserId = userId;
            }

            Int32.TryParse(BookFactory.CreateNewBook(Db, "test book", "test author"), out int bookId);
            Int32.TryParse(ListFactory.CreateNewList(Db, UserId, "test list"), out int listId);

            BookId = bookId;
            ListId = listId;
        }

        ~ItemCRUDOperations()
        {
            BookFactory.DeleteBook(Db, BookId);
            ListFactory.DeleteList(Db, ListId);
            UserFactory.DeleteUser(Db, UserId);
        }

        [Test]
        public void TestCreateNewItem()
        {
            if (Int32.TryParse(ItemFactory.CreateNewItem(Db, BookId, ListId), out int id))
            {
                var testItem = ItemFactory.LoadSingle(Db, id);

                Assert.IsNotNull(testItem);
                Assert.AreEqual(BookId, testItem.Book.Id);
                Assert.AreEqual(ListId, testItem.ListId);

                ItemFactory.DeleteItem(Db, id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestLoadAll()
        {
            var testItemsList = ItemFactory.LoadAll(Db);
            //var testItem = testItemsList.Find(item => item.Id == 1);

            Assert.IsNotNull(testItemsList);
            //Assert.IsNotNull(testItem);
            //Assert.AreEqual(2, testItem.Book.Id);
        }

        [Test]
        public void TestDeleteItem()
        {
            if (Int32.TryParse(ItemFactory.CreateNewItem(Db, BookId, ListId), out int id))
            {
                ItemFactory.DeleteItem(Db, id);

                var testItem = ItemFactory.LoadSingle(Db, id);

                Assert.IsNull(testItem);
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}
