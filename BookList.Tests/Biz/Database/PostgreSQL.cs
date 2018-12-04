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

            var results = db.Take("test").Execute();

            Assert.IsNotNull(db.ResultSet);
            Assert.AreEqual("Marla", results[1][0]);
        }

        [Test]
        public void TestWhere()
        {
            var db = new PostgreSQLConnection();

            var results = db.Take("test").Where(new ColumnValuePairing("name", "Susan"), new ColumnValuePairing("id", 2)).Execute();

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results[0].Count);
            Assert.AreEqual("Susan", results[1][0]);
        }

        [Test]
        public void TestOrderBy()
        {
            var db = new PostgreSQLConnection();

            // Test default orderby
            var results = db.Take("test").OrderBy("name").Execute();

            Assert.IsNotNull(results);
            Assert.AreEqual(5, results[0].Count);
            Assert.AreEqual("Susan", results[1][0]);

            // Test orderby ascending
            results = db.Take("test").OrderBy("name", "asc").Execute();

            Assert.IsNotNull(results);
            Assert.AreEqual(5, results[0].Count);
            Assert.AreEqual("Jenna", results[1][0]);
        }

        [Test]
        public void TestLimit()
        {
            var db = new PostgreSQLConnection();

            var results = db.Take("test").OrderBy("name").Limit(1).Execute();

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results[0].Count);
            Assert.AreEqual("Susan", results[1][0]);
        }
    }
}
