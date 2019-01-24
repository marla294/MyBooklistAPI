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
            if (Int32.TryParse(ListFactory.CreateNewList(Db, UserToken), out int id))
            {
                var testList = ListFactory.LoadSingle(Db, id);

                Assert.IsNotNull(testList);
                Assert.AreEqual("New List", testList.Name);

                ListFactory.DeleteList(Db, id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestCreateNewListBlankName()
        {
            var list = ListFactory.CreateNewList(Db, UserToken, "");

            if (list == null)
            {
                Assert.Pass();
            }
            else
            {
                if (Int32.TryParse(list, out int id))
                {
                    ListFactory.DeleteList(Db, id);
                }
                Assert.Fail();
            }
        }

        [Test]
        public void TestCreateNewListTooLongName()
        {
            if (Int32.TryParse(ListFactory.CreateNewList(Db, UserToken, "asdf123456asdf123456asdf123456asdf"), out int id))
            {
                var testList = ListFactory.LoadSingle(Db, id);

                Assert.IsNotNull(testList);
                Assert.AreEqual("asdf123456asdf123456asdf123456", testList.Name);

                ListFactory.DeleteList(Db, id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestCreateNewListInvalidName()
        {
            var list = ListFactory.CreateNewList(Db, UserToken, "#$%^asdf");

            if (list == null)
            {
                Assert.Pass();
            }
            else
            {
                if (Int32.TryParse(list, out int id)) 
                {
                    ListFactory.DeleteList(Db, id);
                }
                Assert.Fail();
            }
        }

        [Test]
        public void TestLoadAll()
        {
            if (Int32.TryParse(ListFactory.CreateNewList(Db, UserToken), out int id))
            {
                var testListList = ListFactory.LoadAll(Db);
                var testList = testListList.Find(list => list.Id == id);

                Assert.IsNotNull(testListList);
                Assert.IsNotNull(testList);

                ListFactory.DeleteList(Db, id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestUpdateList()
        {
            if (Int32.TryParse(ListFactory.CreateNewList(Db, UserToken), out int id))
            {
                var testListName = "Updated Name";

                ListFactory.UpdateListName(Db, id, testListName);
                var testList = ListFactory.LoadSingle(Db, id);

                Assert.IsNotNull(testList);
                Assert.AreEqual(testListName, testList.Name);

                ListFactory.DeleteList(Db, id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestUpdateListBlankName()
        {
            if (Int32.TryParse(ListFactory.CreateNewList(Db, UserToken), out int id))
            {
                // First update to normal name
                var testListName = "Normal Name";
                ListFactory.UpdateListName(Db, id, testListName);

                // Then try to update it to be blank
                ListFactory.UpdateListName(Db, id, "");

                var testList = ListFactory.LoadSingle(Db, id);

                // Make sure the name of the list is the old name, not blank
                Assert.IsNotNull(testList);
                Assert.AreEqual(testListName, testList.Name);

                ListFactory.DeleteList(Db, id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestUpdateListTooLongName()
        {
            if (Int32.TryParse(ListFactory.CreateNewList(Db, UserToken), out int id))
            {
                var testListName = "asdf123456asdf123456asdf123456asdf";

                ListFactory.UpdateListName(Db, id, testListName);
                var testList = ListFactory.LoadSingle(Db, id);

                Assert.IsNotNull(testList);
                Assert.AreEqual("asdf123456asdf123456asdf123456", testList.Name);

                ListFactory.DeleteList(Db, id);
            }
            else
            {
                Assert.Fail();
            }
        }

        // In the application, the client side strips the invalid characters before it gets to the server
        // But if invalid characters get to the server somehow, then we want to just cancel updating the list altogether
        [Test]
        public void TestUpdateListInvalidName()
        {
            if (Int32.TryParse(ListFactory.CreateNewList(Db, UserToken), out int id))
            {
                ListFactory.UpdateListName(Db, id, "*&^%asdf");
                var testList = ListFactory.LoadSingle(Db, id);

                Assert.IsNotNull(testList);
                Assert.AreEqual("New List", testList.Name);

                ListFactory.DeleteList(Db, id);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestDeleteList()
        {
            if (Int32.TryParse(ListFactory.CreateNewList(Db, UserToken), out int id))
            {
                ListFactory.DeleteList(Db, id);

                var testList = ListFactory.LoadSingle(Db, id);

                Assert.IsNull(testList);
            }
            else
            {
                Assert.Fail();
            }
        }

    }
}
