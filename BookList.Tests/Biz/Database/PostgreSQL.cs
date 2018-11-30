using System;
using NUnit.Framework;
using BookList.Biz.Database;

namespace BookList.Tests.Biz.Database
{
    [TestFixture]
    public class PostgreSQL
    {
        [Test]
        public void TestTake()
        {
            var db = new PostgreSQLConnection();

            var results = db.Take("test");

            Assert.IsNotNull(db.ResultSet);
            Assert.AreEqual("Marla", db.ResultSet[1][0]);
        }

        [Test]
        public void TestWhere()
        {
            var db = new PostgreSQLConnection();

            var results = db.Take("test").Where("name", "Marla");

            Assert.IsNotNull(results);
            Assert.AreEqual("Marla", db.ResultSet[1][0]);
        }
    }
}
