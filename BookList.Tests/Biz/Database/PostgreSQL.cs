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

            var results = db.Take(new string[] { "*" }, "test");

            Assert.IsNotNull(results);
            Assert.AreEqual("Marla", results[1][0]);
        }
    }
}
