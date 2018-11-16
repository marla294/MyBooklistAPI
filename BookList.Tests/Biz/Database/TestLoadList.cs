using NUnit.Framework;
using BookList.Biz.Database;

namespace BookList.Tests.Biz.Database
{
    [TestFixture]
    public class LoadListTests
    {
        [Test]
        public void TestLoadAll()
        {
            var testListList = LoadList.LoadAll();
            var testList = testListList.Find(list => list.Id == 1);

            Assert.IsNotNull(testListList);
            Assert.IsNotNull(testList);
            Assert.AreEqual(1, testListList.Count);
            //Assert.AreEqual("Want To Read (From Database)", testList.Name);
        }
    }
}
