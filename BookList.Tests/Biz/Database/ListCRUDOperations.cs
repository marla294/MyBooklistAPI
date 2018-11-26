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
            if (Int32.TryParse(ListFactory.CreateNewList(), out int id))
            {
                var testListList = ListFactory.LoadAll();
                var testList = testListList.Find(list => list.Id == id);

                Assert.IsNotNull(testListList);
                Assert.IsNotNull(testList);

                ListFactory.DeleteList(id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestUpdateList()
        {
            if (Int32.TryParse(ListFactory.CreateNewList(), out int id))
            {
                var testListName = "Updated Name";

                ListFactory.UpdateListName(id, testListName);
                var testList = ListFactory.LoadSingle(id);

                Assert.IsNotNull(testList);
                Assert.AreEqual(testListName, testList.Name);

                ListFactory.DeleteList(id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestDeleteList()
        {
            if (Int32.TryParse(ListFactory.CreateNewList(), out int id))
            {
                ListFactory.DeleteList(id);

                var testList = ListFactory.LoadAll().Find(list => list.Id == id);

                Assert.IsNull(testList);
            }
            else
            {
                Assert.Fail();
            }
        }

    }
}
