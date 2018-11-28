using NUnit.Framework;
using BookList.Biz.Database;

namespace BookList.Tests.Biz.Database
{
    [TestFixture]
    public class LoadBookTests
    {
        [Test]
        public void TestLoadAll()
        {
            var testBookList = LoadBook.LoadAll(new PostgreSQLConnection());
            var testBook = testBookList.Find(book => book.Id == 2);

            Assert.IsNotNull(testBookList);
            Assert.IsNotNull(testBook);
            Assert.AreEqual(3, testBookList.Count);
            Assert.AreEqual("all the light we cannot see", testBook.Title);
        }
    }
}
