using NUnit.Framework;
using BookList.Biz.Database;

namespace BookList.Tests.Biz.Database
{
    [TestFixture]
    public class LoadItemsTests
    {
        [Test]
        public void TestLoadAll()
        {
            var testItemsList = ItemFactory.LoadAll();
            var testItem = testItemsList.Find(item => item.Id == 1);

            Assert.IsNotNull(testItemsList);
            Assert.IsNotNull(testItem);
            Assert.AreEqual(2, testItem.Book.Id);
        }
    }
}
