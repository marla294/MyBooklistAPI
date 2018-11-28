using System;
using NUnit.Framework;
using BookList.Biz.Database;

namespace BookList.Tests.Biz.Database
{
    [TestFixture]
    public class ListCRUDOperations
    {
        [Test]
        public void TestCreateNewList()
        {
            var db = new PostgreSQLConnection();

            if (Int32.TryParse(ListFactory.CreateNewList(db), out int id))
            {
                var testList = ListFactory.LoadSingle(db, id);

                Assert.IsNotNull(testList);
                Assert.AreEqual("New List", testList.Name);

                ListFactory.DeleteList(db, id);
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

            if (Int32.TryParse(ListFactory.CreateNewList(db), out int id))
            {
                var testListList = ListFactory.LoadAll(db);
                var testList = testListList.Find(list => list.Id == id);

                Assert.IsNotNull(testListList);
                Assert.IsNotNull(testList);

                ListFactory.DeleteList(db, id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestUpdateList()
        {
            var db = new PostgreSQLConnection();

            if (Int32.TryParse(ListFactory.CreateNewList(db), out int id))
            {
                var testListName = "Updated Name";

                ListFactory.UpdateListName(db, id, testListName);
                var testList = ListFactory.LoadSingle(db, id);

                Assert.IsNotNull(testList);
                Assert.AreEqual(testListName, testList.Name);

                ListFactory.DeleteList(db, id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestDeleteList()
        {
            var db = new PostgreSQLConnection();

            if (Int32.TryParse(ListFactory.CreateNewList(db), out int id))
            {
                ListFactory.DeleteList(db, id);

                var testList = ListFactory.LoadSingle(db, id);

                Assert.IsNull(testList);
            }
            else
            {
                Assert.Fail();
            }
        }

    }
}
