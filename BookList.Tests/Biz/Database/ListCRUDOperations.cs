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
            if (Int32.TryParse(ListFactory.CreateNewList(Db, UserId), out int id))
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
        public void TestLoadAll()
        {
            if (Int32.TryParse(ListFactory.CreateNewList(Db, UserId), out int id))
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
            if (Int32.TryParse(ListFactory.CreateNewList(Db, UserId), out int id))
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
        public void TestDeleteList()
        {
            if (Int32.TryParse(ListFactory.CreateNewList(Db, UserId), out int id))
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
