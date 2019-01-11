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

                UserFactory.DeleteUser(Db, testUser.Token);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestCreateInvalidUserDupeUsername()
        {
            var userToken1 = UserFactory.CreateNewUser(Db, "test", "testusername", "testpassword");
            var userToken2 = UserFactory.CreateNewUser(Db, "test", "testusername", "testpassword");

            if (userToken2 != null)
            {
                UserFactory.DeleteUser(Db, userToken2);
                Assert.Fail();
            }
            else
            {
                var testUser1 = UserFactory.LoadSingleByToken(userToken1);

                Assert.IsNotNull(testUser1);
                Assert.AreEqual("test", testUser1.Name);
                Assert.AreEqual("testusername", testUser1.Username);
                Assert.AreEqual(true, UserFactory.ConfirmUserPassword("testusername", "testpassword"));
            }

            UserFactory.DeleteUser(Db, userToken1);
        }

        [Test]
        public void TestCreateInvalidUserEmptyUsername()
        {
            var userToken = UserFactory.CreateNewUser(Db, "test", "", "testpassword");

            if (userToken != null)
            {
                UserFactory.DeleteUser(Db, userToken);
                Assert.Fail();
            }
            else
            {
                Assert.Pass();
            }
        }

        [Test]
        public void TestCreateInvalidUserEmptyPassword()
        {
            var userToken = UserFactory.CreateNewUser(Db, "test", "testusername", "");

            if (userToken != null)
            {
                UserFactory.DeleteUser(Db, userToken);
                Assert.Fail();
            }
            else
            {
                Assert.Pass();
            }
        }

        [Test]
        public void TestCreateInvalidUserTooShortUsername()
        {
            var userToken = UserFactory.CreateNewUser(Db, "test", "test", "testpassword");

            if (userToken != null)
            {
                UserFactory.DeleteUser(Db, userToken);
                Assert.Fail();
            }
            else
            {
                Assert.Pass();
            }
        }

        [Test]
        public void TestCreateInvalidUserTooShortPassword()
        {
            var userToken = UserFactory.CreateNewUser(Db, "test", "testusername", "test");

            if (userToken != null)
            {
                UserFactory.DeleteUser(Db, userToken);
                Assert.Fail();
            }
            else
            {
                Assert.Pass();
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
