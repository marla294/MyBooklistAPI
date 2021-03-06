﻿using System;
using NUnit.Framework;
using BookList.Biz.Database;
using BookList.Biz.Models;

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
            UserFactory.DeleteUser(Db, userToken2);
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
        public void TestCreateInvalidUserTooLongUsername()
        {
            var userToken = UserFactory.CreateNewUser(Db, "test", "testasdf12testasdf12testasdf12testasdf12asdf", "testpassword");

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
        public void TestCreateInvalidUserInvalidCharactersUsername()
        {
            var userToken = UserFactory.CreateNewUser(Db, "test", "Test@#$%^asdf", "testpassword");

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
        public void TestCreateInvalidUserTooLongName()
        {
            var userToken = UserFactory.CreateNewUser(Db, "asdfgasdfgasdfgasdfgasdfgasdfgasdfgasdfgasdfgasdfg", "testusername", "testpassword");

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
        public void TestCreateInvalidUserNonAlphaName()
        {
            var userToken = UserFactory.CreateNewUser(Db, "asdf1234", "testusername", "testpassword");

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
        public void TestCreateInvalidUserTooLongPassword()
        {
            var userToken = UserFactory.CreateNewUser(Db, "test", "testusername", "testasdf12testasdf12testasdf12testasdf12asdf");

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

        [Test]
        public void TestConfirmUserPassword()
        {
            var testUserToken = UserFactory.CreateNewUser(Db, "test", "testusername", "testpassword");
            var testUser = UserFactory.LoadSingleByToken(testUserToken);

            if (UserFactory.ConfirmUserPassword("testusername", "testpassword"))
            {
                UserFactory.DeleteUser(Db, testUserToken);
                Assert.Pass();
            }
            else
            {
                UserFactory.DeleteUser(Db, testUserToken);
                Assert.Fail();
            }
        }

        [Test]
        public void TestUpdateUserFirstName()
        {
            var testUserToken = UserFactory.CreateNewUser(Db, "test", "testusername", "testpassword");
            UserFactory.UpdateFirstName(Db, testUserToken, "UpdatedName");
            User updatedUser = UserFactory.LoadSingleByToken(testUserToken);

            Assert.AreEqual("UpdatedName", updatedUser.Name);

            UserFactory.DeleteUser(Db, testUserToken);
        }

        [Test]
        public void TestUpdateUserFirstNameBlank()
        {
            var testUserToken = UserFactory.CreateNewUser(Db, "test", "testusername", "testpassword");
            UserFactory.UpdateFirstName(Db, testUserToken, "");
            User updatedUser = UserFactory.LoadSingleByToken(testUserToken);

            // Name shouldn't be updated since it is blank
            Assert.AreEqual("test", updatedUser.Name);

            UserFactory.DeleteUser(Db, testUserToken);
        }

        [Test]
        public void TestUpdateUserFirstNameTooLong()
        {
            var testUserToken = UserFactory.CreateNewUser(Db, "test", "testusername", "testpassword");
            UserFactory.UpdateFirstName(Db, testUserToken, "asdfasdfqwasdfasdfqwasdfasdfqwasdfasdfqwsdfg");
            User updatedUser = UserFactory.LoadSingleByToken(testUserToken);

            // Name shouldn't be updated since it was too long (will warn on the client side)
            Assert.AreEqual("test", updatedUser.Name);

            UserFactory.DeleteUser(Db, testUserToken);
        }

        [Test]
        public void TestUpdateUserFirstNameInvalid()
        {
            var testUserToken = UserFactory.CreateNewUser(Db, "test", "testusername", "testpassword");
            UserFactory.UpdateFirstName(Db, testUserToken, "@#$%^&*(");
            User updatedUser = UserFactory.LoadSingleByToken(testUserToken);

            // Name shouldn't be updated since there were invalid characters
            Assert.AreEqual("test", updatedUser.Name);

            UserFactory.DeleteUser(Db, testUserToken);
        }

        [Test]
        public void TestIsUserTokenInDbFalse()
        {
            if (UserFactory.LoadSingleByToken(new Random().Next(0, 1000000000).ToString()) == null)
            {
                Assert.Pass();
            } else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestIsUserTokenInDbTrue()
        {
            var testUserToken = UserFactory.CreateNewUser(Db, "test", "testusername", "testpassword");

            if (UserFactory.LoadSingleByToken(testUserToken) == null)
            {
                UserFactory.DeleteUser(Db, testUserToken);
                Assert.Fail();
            }
            else
            {
                UserFactory.DeleteUser(Db, testUserToken);
                Assert.Pass();
            }
        }
    }
}
