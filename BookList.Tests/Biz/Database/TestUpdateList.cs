using System;
using NUnit.Framework;
using BookList.Biz.Database;

namespace BookList.Tests.Biz.Database
{
    [TestFixture]
    public class UpdateListTests
    {
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
