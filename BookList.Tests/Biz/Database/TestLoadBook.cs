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

            Assert.AreEqual(testBookList.Count, 3);
        }
    }
}
