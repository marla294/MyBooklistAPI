using NUnit.Framework;
using BookList.Biz.Database;

namespace BookList.Tests.Biz.Database
{
    [TestFixture]
    public class UserCRUDOperations
    {
        [Test]
        public void TestLoadAll()
        {
            var testUserList = UserFactory.LoadAll();
            var testUser = testUserList.Find(user => user.Id == 1);

            Assert.IsNotNull(testUserList);
            Assert.IsNotNull(testUser);
            Assert.AreEqual(2, testUserList.Count);
            Assert.AreEqual("Marla", testUser.Name);
        }
    }
}
