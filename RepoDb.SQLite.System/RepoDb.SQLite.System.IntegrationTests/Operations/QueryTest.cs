﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoDb.Extensions;
using RepoDb.SQLite.System.IntegrationTests.Models;
using RepoDb.SQLite.System.IntegrationTests.Setup;
using System;
using System.Data.SQLite;
using System.Linq;

namespace RepoDb.SQLite.System.IntegrationTests.Operations.SDS
{
    [TestClass]
    public class QueryTest
    {
        [TestInitialize]
        public void Initialize()
        {
            Database.Initialize();
            Cleanup();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Database.Cleanup();
        }

        #region DataEntity

        #region Sync

        [TestMethod]
        public void TestSqLiteConnectionQueryViaPrimaryKey()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();

                // Act
                var result = connection.Query<SdsCompleteTable>(table.Id).First();

                // Assert
                Helper.AssertPropertiesEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryViaExpression()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();

                // Act
                var result = connection.Query<SdsCompleteTable>(e => e.Id == table.Id).First();

                // Assert
                Helper.AssertPropertiesEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryViaDynamic()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();

                // Act
                var result = connection.Query<SdsCompleteTable>(new { table.Id }).First();

                // Assert
                Helper.AssertPropertiesEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryViaQueryField()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();

                // Act
                var result = connection.Query<SdsCompleteTable>(new QueryField("Id", table.Id)).First();

                // Assert
                Helper.AssertPropertiesEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryViaQueryFields()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();
                var queryFields = new[]
                {
                    new QueryField("Id", table.Id),
                    new QueryField("ColumnInt", table.ColumnInt)
                };

                // Act
                var result = connection.Query<SdsCompleteTable>(queryFields).First();

                // Assert
                Helper.AssertPropertiesEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryViaQueryGroup()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();
                var queryFields = new[]
                {
                    new QueryField("Id", table.Id),
                    new QueryField("ColumnInt", table.ColumnInt)
                };
                var queryGroup = new QueryGroup(queryFields);

                // Act
                var result = connection.Query<SdsCompleteTable>(queryGroup).First();

                // Assert
                Helper.AssertPropertiesEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryWithTop()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                var result = connection.Query<SdsCompleteTable>((object)null,
                    top: 2);

                // Assert
                Assert.AreEqual(2, result.Count());
                result.AsList().ForEach(item => Helper.AssertPropertiesEquality(tables.First(e => e.Id == item.Id), item));
            }
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void ThrowExceptionQueryWithHints()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();

                // Act
                connection.Query<SdsCompleteTable>((object)null,
                    hints: "WhatEver");
            }
        }

        #endregion

        #region Async

        [TestMethod]
        public void TestSqLiteConnectionQueryAsyncViaPrimaryKey()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();

                // Act
                var result = connection.QueryAsync<SdsCompleteTable>(table.Id).Result.First();

                // Assert
                Helper.AssertPropertiesEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryAsyncViaExpression()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();

                // Act
                var result = connection.QueryAsync<SdsCompleteTable>(e => e.Id == table.Id).Result.First();

                // Assert
                Helper.AssertPropertiesEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryAsyncViaDynamic()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();

                // Act
                var result = connection.QueryAsync<SdsCompleteTable>(new { table.Id }).Result.First();

                // Assert
                Helper.AssertPropertiesEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryAsyncViaQueryField()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();

                // Act
                var result = connection.QueryAsync<SdsCompleteTable>(new QueryField("Id", table.Id)).Result.First();

                // Assert
                Helper.AssertPropertiesEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryAsyncViaQueryFields()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();
                var queryFields = new[]
                {
                    new QueryField("Id", table.Id),
                    new QueryField("ColumnInt", table.ColumnInt)
                };

                // Act
                var result = connection.QueryAsync<SdsCompleteTable>(queryFields).Result.First();

                // Assert
                Helper.AssertPropertiesEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryAsyncViaQueryGroup()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();
                var queryFields = new[]
                {
                    new QueryField("Id", table.Id),
                    new QueryField("ColumnInt", table.ColumnInt)
                };
                var queryGroup = new QueryGroup(queryFields);

                // Act
                var result = connection.QueryAsync<SdsCompleteTable>(queryGroup).Result.First();

                // Assert
                Helper.AssertPropertiesEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryAsyncWithTop()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                var result = connection.QueryAsync<SdsCompleteTable>((object)null,
                    top: 2).Result;

                // Assert
                Assert.AreEqual(2, result.Count());
                result.AsList().ForEach(item => Helper.AssertPropertiesEquality(tables.First(e => e.Id == item.Id), item));
            }
        }

        [TestMethod, ExpectedException(typeof(AggregateException))]
        public void ThrowExceptionQueryAsyncWithHints()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();

                // Act
                connection.QueryAsync<SdsCompleteTable>((object)null,
                    hints: "WhatEver").Wait();
            }
        }

        #endregion

        #endregion

        #region TableName

        #region Sync

        [TestMethod]
        public void TestSqLiteConnectionQueryViaTableNameViaPrimaryKey()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();

                // Act
                var result = connection.Query(ClassMappedNameCache.Get<SdsCompleteTable>(), table.Id).First();

                // Assert
                Helper.AssertMembersEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryViaTableNameViaDynamic()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();

                // Act
                var result = connection.Query(ClassMappedNameCache.Get<SdsCompleteTable>(), new { table.Id }).First();

                // Assert
                Helper.AssertMembersEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryViaTableNameViaQueryField()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();

                // Act
                var result = connection.Query(ClassMappedNameCache.Get<SdsCompleteTable>(), new QueryField("Id", table.Id)).First();

                // Assert
                Helper.AssertMembersEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryViaTableNameViaQueryFields()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();
                var queryFields = new[]
                {
                    new QueryField("Id", table.Id),
                    new QueryField("ColumnInt", table.ColumnInt)
                };

                // Act
                var result = connection.Query(ClassMappedNameCache.Get<SdsCompleteTable>(), queryFields).First();

                // Assert
                Helper.AssertMembersEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryViaTableNameViaQueryGroup()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();
                var queryFields = new[]
                {
                    new QueryField("Id", table.Id),
                    new QueryField("ColumnInt", table.ColumnInt)
                };
                var queryGroup = new QueryGroup(queryFields);

                // Act
                var result = connection.Query(ClassMappedNameCache.Get<SdsCompleteTable>(), queryGroup).First();

                // Assert
                Helper.AssertMembersEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryViaTableNameWithTop()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                var result = connection.Query(ClassMappedNameCache.Get<SdsCompleteTable>(),
                    (object)null,
                    top: 2);

                // Assert
                Assert.AreEqual(2, result.Count());
                result.AsList().ForEach(item => Helper.AssertPropertiesEquality(tables.First(e => e.Id == item.Id), item));
            }
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void ThrowExceptionQueryViaTableNameWithHints()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();

                // Act
                connection.Query(ClassMappedNameCache.Get<SdsCompleteTable>(),
                    (object)null,
                    hints: "WhatEver");
            }
        }

        #endregion

        #region Async

        [TestMethod]
        public void TestSqLiteConnectionQueryAsyncViaTableNameViaPrimaryKey()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();

                // Act
                var result = connection.QueryAsync(ClassMappedNameCache.Get<SdsCompleteTable>(), table.Id).Result.First();

                // Assert
                Helper.AssertMembersEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryAsyncViaTableNameViaDynamic()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();

                // Act
                var result = connection.QueryAsync(ClassMappedNameCache.Get<SdsCompleteTable>(), new { table.Id }).Result.First();

                // Assert
                Helper.AssertMembersEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryAsyncViaTableNameViaQueryField()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();

                // Act
                var result = connection.QueryAsync(ClassMappedNameCache.Get<SdsCompleteTable>(), new QueryField("Id", table.Id)).Result.First();

                // Assert
                Helper.AssertMembersEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryAsyncViaTableNameViaQueryFields()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();
                var queryFields = new[]
                {
                    new QueryField("Id", table.Id),
                    new QueryField("ColumnInt", table.ColumnInt)
                };

                // Act
                var result = connection.QueryAsync(ClassMappedNameCache.Get<SdsCompleteTable>(), queryFields).Result.First();

                // Assert
                Helper.AssertMembersEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryAsyncViaTableNameViaQueryGroup()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();
                var queryFields = new[]
                {
                    new QueryField("Id", table.Id),
                    new QueryField("ColumnInt", table.ColumnInt)
                };
                var queryGroup = new QueryGroup(queryFields);

                // Act
                var result = connection.QueryAsync(ClassMappedNameCache.Get<SdsCompleteTable>(), queryGroup).Result.First();

                // Assert
                Helper.AssertMembersEquality(table, result);
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionQueryAsyncViaTableNameWithTop()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                var result = connection.QueryAsync(ClassMappedNameCache.Get<SdsCompleteTable>(),
                    (object)null,
                    top: 2).Result;

                // Assert
                Assert.AreEqual(2, result.Count());
                result.AsList().ForEach(item => Helper.AssertPropertiesEquality(tables.First(e => e.Id == item.Id), item));
            }
        }

        [TestMethod, ExpectedException(typeof(AggregateException))]
        public void ThrowExceptionQueryAsyncViaTableNameWithHints()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var table = Database.CreateSdsCompleteTables(1, connection).First();

                // Act
                connection.QueryAsync(ClassMappedNameCache.Get<SdsCompleteTable>(),
                    (object)null,
                    hints: "WhatEver").Wait();
            }
        }

        #endregion

        #endregion
    }
}
