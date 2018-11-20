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
            var testListListOrig = LoadList.LoadAll();
            var testListOrig = testListListOrig.Find(list => list.Id == 1);
            var oldName = testListOrig.Name;

            UpdateList.UpdateListName(1, "Updated Name");

            var testListList = LoadList.LoadAll();
            var testList = testListList.Find(list => list.Id == 1);

            Assert.IsNotNull(testListList);
            Assert.IsNotNull(testList);
            Assert.AreEqual("Updated Name", testList.Name);

            UpdateList.UpdateListName(1, $"{oldName}");
        }
    }
}
