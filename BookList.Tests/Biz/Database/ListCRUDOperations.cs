using System;
using NUnit.Framework;
using BookList.Biz.Database;

namespace BookList.Tests.Biz.Database
{
    [TestFixture]
    public class ListCRUDOperations
    {
        PostgreSQLConnection Db { get; set; }
        int UserId { get; set; }
        string UserToken { get; set; }

        public ListCRUDOperations()
        {
            Db = new PostgreSQLConnection();

            var userToken = UserFactory.CreateNewUser(Db, "testuser", "testuserlist", "password");

            if (userToken != null) {
                UserId = UserFactory.LoadSingleByToken(userToken).Id;
                UserToken = userToken;
            }
        }

        ~ListCRUDOperations()
        {
            UserFactory.DeleteUser(Db, UserToken);
        }


        [Test]
        public void TestCreateNewList()
        {
            var id = ListFactory.CreateNewList(Db, UserToken);

            if (id == null)
            {
                Assert.Fail();
            }
            else
            {
                var testList = ListFactory.LoadSingle(Db, (int)id);

                Assert.IsNotNull(testList);
                Assert.AreEqual("New List", testList.Name);

                ListFactory.DeleteList(Db, (int)id);
            }
        }

        [Test]
        public void TestCreateNewListBlankName()
        {
            var id = ListFactory.CreateNewList(Db, UserToken, "   ");

            if (id == null)
            {
                Assert.Pass();
            }
            else
            {
                ListFactory.DeleteList(Db, (int)id);
                Assert.Fail();
            }
        }

        [Test]
        public void TestCreateNewListTooLongName()
        {
            var id = ListFactory.CreateNewList(Db, UserToken, "asdf123456asdf123456asdf123456asdf");

            if (id == null)
            {
                Assert.Fail();
            }
            else
            {
                var testList = ListFactory.LoadSingle(Db, (int)id);

                Assert.IsNotNull(testList);
                Assert.AreEqual("asdf123456asdf123456asdf123456", testList.Name);

                ListFactory.DeleteList(Db, (int)id);
            }
        }

        [Test]
        public void TestCreateNewListInvalidName()
        {
            var id = ListFactory.CreateNewList(Db, UserToken, "#$%^asdf");

            if (id == null)
            {
                Assert.Pass();
            }
            else
            {
                ListFactory.DeleteList(Db, (int)id);
                Assert.Fail();
            }
        }

        [Test]
        public void TestLoadAll()
        {
            var id = ListFactory.CreateNewList(Db, UserToken);

            if (id == null)
            {
                Assert.Fail();
            }
            else
            {
                var testListList = ListFactory.LoadAll(Db);
                var testList = testListList.Find(list => list.Id == id);

                Assert.IsNotNull(testListList);
                Assert.IsNotNull(testList);

                ListFactory.DeleteList(Db, (int)id);
            }
        }

        [Test]
        public void TestUpdateList()
        {
            var id = ListFactory.CreateNewList(Db, UserToken);

            if (id == null)
            {
                Assert.Fail();
            }
            else 
            {
                var testListName = "Updated Name";

                ListFactory.UpdateListName(Db, (int)id, testListName);
                var testList = ListFactory.LoadSingle(Db, (int)id);

                Assert.IsNotNull(testList);
                Assert.AreEqual(testListName, testList.Name);

                ListFactory.DeleteList(Db, (int)id);
            }
        }

        [Test]
        public void TestUpdateListBlankName()
        {
            var id = ListFactory.CreateNewList(Db, UserToken);

            if (id == null)
            {
                Assert.Fail();
            }
            else 
            {
                // First update to normal name
                var testListName = "Normal Name";
                ListFactory.UpdateListName(Db, (int)id, testListName);

                // Then try to update it to be blank
                ListFactory.UpdateListName(Db, (int)id, "");

                var testList = ListFactory.LoadSingle(Db, (int)id);

                // Make sure the name of the list is the old name, not blank
                Assert.IsNotNull(testList);
                Assert.AreEqual(testListName, testList.Name);

                ListFactory.DeleteList(Db, (int)id);
            }
        }

        [Test]
        public void TestUpdateListTooLongName()
        {
            var id = ListFactory.CreateNewList(Db, UserToken);

            if (id == null)
            {
                Assert.Fail();
            }
            else
            {
                var testListName = "asdf123456asdf123456asdf123456asdf";

                ListFactory.UpdateListName(Db, (int)id, testListName);
                var testList = ListFactory.LoadSingle(Db, (int)id);

                Assert.IsNotNull(testList);
                Assert.AreEqual("asdf123456asdf123456asdf123456", testList.Name);

                ListFactory.DeleteList(Db, (int)id);
            }
        }

        // In the application, the client side strips the invalid characters before it gets to the server
        // But if invalid characters get to the server somehow, then we want to just cancel updating the list altogether
        [Test]
        public void TestUpdateListInvalidName()
        {
            var id = ListFactory.CreateNewList(Db, UserToken);

            if (id == null)
            {
                Assert.Fail();
            }
            else
            {
                ListFactory.UpdateListName(Db, (int)id, "*&^%asdf");
                var testList = ListFactory.LoadSingle(Db, (int)id);
               
                Assert.IsNotNull(testList);
                Assert.AreEqual("New List", testList.Name);

                ListFactory.DeleteList(Db, (int)id);
            }
        }

        [Test]
        public void TestDeleteList()
        {
            var id = ListFactory.CreateNewList(Db, UserToken);

            if (id == null)
            {
                Assert.Fail();
            }
            else
            {
                ListFactory.DeleteList(Db, (int)id);
                var testList = ListFactory.LoadSingle(Db, (int)id);
                Assert.IsNull(testList);
            }
        }

    }
}
