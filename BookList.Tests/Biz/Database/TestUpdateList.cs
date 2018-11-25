using System;
using NUnit.Framework;
using BookList.Biz.Database;

namespace BookList.Tests.Biz.Database
{
    [TestFixture]
    public class UpdateListTests
    {
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

        [Test]
        public void TestUpdateList()
        {
            if (Int32.TryParse(LoadList.CreateNewList(), out int id)) 
            {
                LoadList.UpdateListName(id, "Updated Name");
                var testList = LoadList.LoadSingle(id);

                Assert.IsNotNull(testList);
                Assert.AreEqual("Updated Name", testList.Name);

                LoadList.DeleteList(id);
            }
            else
            {
                Assert.Fail();
            }
        }

    }
}
