using NUnit.Framework;
using BookList.Biz.Database;

namespace BookList.Tests
{
    [TestFixture]
    public class LoadBookTests
    {
        [Test]
        public void TestLoadBookLoadAll()
        {
            var testBookList = LoadBook.LoadAll();
            var testBook = testBookList.Find(book => book.Id == 2);

            Assert.IsNotNull(testBookList);
            Assert.IsNotNull(testBook);
            Assert.AreEqual(3, testBookList.Count);
            Assert.AreEqual("all the light we cannot see", testBook.Name);
        }
    }
}
