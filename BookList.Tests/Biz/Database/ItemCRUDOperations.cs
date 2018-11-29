using NUnit.Framework;
using BookList.Biz.Database;

namespace BookList.Tests.Biz.Database
{
    [TestFixture]
    public class ItemCRUDOperations
    {
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
    }
}
