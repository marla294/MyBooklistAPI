using System;
using NUnit.Framework;
using BookList.Biz.Database;

namespace BookList.Tests.Biz.Database
{
    [TestFixture]
    public class UserCRUDOperations
    {
        [Test]
        public void TestCreateNewUser()
        {
            var db = new PostgreSQLConnection();

            if (Int32.TryParse(UserFactory.CreateNewUser(db, "test", "testusername", "testpassword"), out int id))
            {
                var testUser = UserFactory.LoadSingle(id);

                Assert.IsNotNull(testUser);
                Assert.AreEqual("test", testUser.Name);
                Assert.AreEqual("testusername", testUser.Username);
                Assert.AreEqual("testpassword", testUser.Password);

                UserFactory.DeleteUser(db, id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestLoadAll()
        {
            var db = new PostgreSQLConnection();

            var testUserList = UserFactory.LoadAll(db);
            var testUser = testUserList.Find(user => user.Id == 1);

            Assert.IsNotNull(testUserList);
            Assert.IsNotNull(testUser);
            Assert.AreEqual(2, testUserList.Count);
            Assert.AreEqual("Marla", testUser.Name);
        }
    }
}
