using System;
using NUnit.Framework;
using BookList.Biz.Database;

namespace BookList.Tests.Biz.Database
{
    [TestFixture]
    public class UserCRUDOperations
    {
        PostgreSQLConnection Db { get; set; }

        public UserCRUDOperations() {
            Db = new PostgreSQLConnection();
        }

        [Test]
        public void TestCreateNewUser()
        {
            var userToken = UserFactory.CreateNewUser(Db, "test", "testusername", "testpassword");

            if (userToken != null)
            {
                var testUser = UserFactory.LoadSingleByToken(userToken);

                Assert.IsNotNull(testUser);
                Assert.AreEqual("test", testUser.Name);
                Assert.AreEqual("testusername", testUser.Username);
                Assert.AreEqual(true, UserFactory.ConfirmUserPassword("testusername", "testpassword"));

                UserFactory.DeleteUser(Db, testUser.Id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestLoadAll()
        {
            var testUserList = UserFactory.LoadAll(Db);
            var testUser = testUserList.Find(user => user.Id == 1);

            Assert.IsNotNull(testUserList);
            Assert.IsNotNull(testUser);
            Assert.AreEqual("Marla", testUser.Name);
        }
    }
}
