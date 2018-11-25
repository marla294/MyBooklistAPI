using System;
using NUnit.Framework;
using BookList.Biz.Database;

namespace BookList.Tests.Biz.Database
{
    [TestFixture]
    public class ListCRUDOperations
    {
        [Test]
        public void TestLoadAll()
        {
            if (Int32.TryParse(LoadList.CreateNewList(), out int id))
            {
                var testListList = LoadList.LoadAll();
                var testList = testListList.Find(list => list.Id == id);

                Assert.IsNotNull(testListList);
                Assert.IsNotNull(testList);

                LoadList.DeleteList(id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestUpdateList()
        {
            if (Int32.TryParse(LoadList.CreateNewList(), out int id))
            {
                var testListName = "Updated Name";

                LoadList.UpdateListName(id, testListName);
                var testList = LoadList.LoadSingle(id);

                Assert.IsNotNull(testList);
                Assert.AreEqual(testListName, testList.Name);

                LoadList.DeleteList(id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestDeleteList()
        {
            if (Int32.TryParse(LoadList.CreateNewList(), out int id))
            {
                LoadList.DeleteList(id);

                var testList = LoadList.LoadAll().Find(list => list.Id == id);

                Assert.IsNull(testList);
            }
            else
            {
                Assert.Fail();
            }
        }

    }
}
