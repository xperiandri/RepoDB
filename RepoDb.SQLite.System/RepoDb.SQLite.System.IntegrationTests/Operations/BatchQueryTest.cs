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
    public class BatchQueryTest
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
        public void TestSqLiteConnectionBatchQueryFirstBatchAscending()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                var result = connection.BatchQuery<SdsCompleteTable>(0,
                    3,
                    OrderField.Ascending<SdsCompleteTable>(c => c.Id).AsEnumerable(),
                    (object)null);

                // Assert
                Helper.AssertPropertiesEquality(tables.ElementAt(0), result.ElementAt(0));
                Helper.AssertPropertiesEquality(tables.ElementAt(2), result.ElementAt(2));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionBatchQueryFirstBatchDescending()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                var result = connection.BatchQuery<SdsCompleteTable>(0,
                    3,
                    OrderField.Descending<SdsCompleteTable>(c => c.Id).AsEnumerable(),
                    (object)null);

                // Assert
                Helper.AssertPropertiesEquality(tables.ElementAt(9), result.ElementAt(0));
                Helper.AssertPropertiesEquality(tables.ElementAt(7), result.ElementAt(2));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionBatchQueryThirdBatchAscending()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                var result = connection.BatchQuery<SdsCompleteTable>(2,
                    3,
                    OrderField.Ascending<SdsCompleteTable>(c => c.Id).AsEnumerable(),
                    (object)null);

                // Assert
                Helper.AssertPropertiesEquality(tables.ElementAt(6), result.ElementAt(0));
                Helper.AssertPropertiesEquality(tables.ElementAt(8), result.ElementAt(2));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionBatchQueryThirdBatchDescending()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                var result = connection.BatchQuery<SdsCompleteTable>(2,
                    3,
                    OrderField.Descending<SdsCompleteTable>(c => c.Id).AsEnumerable(),
                    (object)null);

                // Assert
                Helper.AssertPropertiesEquality(tables.ElementAt(3), result.ElementAt(0));
                Helper.AssertPropertiesEquality(tables.ElementAt(1), result.ElementAt(2));
            }
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void ThrowExceptionOnSqLiteConnectionBatchQueryWithHints()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                connection.BatchQuery<SdsCompleteTable>(0,
                    3,
                    OrderField.Ascending<SdsCompleteTable>(c => c.Id).AsEnumerable(),
                    (object)null,
                    hints: "WhatEver");
            }
        }

        #endregion

        #region Async

        [TestMethod]
        public void TestSqLiteConnectionBatchQueryAsyncFirstBatchAscending()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                var result = connection.BatchQueryAsync<SdsCompleteTable>(0,
                    3,
                    OrderField.Ascending<SdsCompleteTable>(c => c.Id).AsEnumerable(),
                    (object)null).Result;

                // Assert
                Helper.AssertPropertiesEquality(tables.ElementAt(0), result.ElementAt(0));
                Helper.AssertPropertiesEquality(tables.ElementAt(2), result.ElementAt(2));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionBatchQueryAsyncFirstBatchDescending()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                var result = connection.BatchQueryAsync<SdsCompleteTable>(0,
                    3,
                    OrderField.Descending<SdsCompleteTable>(c => c.Id).AsEnumerable(),
                    (object)null).Result;

                // Assert
                Helper.AssertPropertiesEquality(tables.ElementAt(9), result.ElementAt(0));
                Helper.AssertPropertiesEquality(tables.ElementAt(7), result.ElementAt(2));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionBatchQueryAsyncThirdBatchAscending()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                var result = connection.BatchQueryAsync<SdsCompleteTable>(2,
                    3,
                    OrderField.Ascending<SdsCompleteTable>(c => c.Id).AsEnumerable(),
                    (object)null).Result;

                // Assert
                Helper.AssertPropertiesEquality(tables.ElementAt(6), result.ElementAt(0));
                Helper.AssertPropertiesEquality(tables.ElementAt(8), result.ElementAt(2));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionBatchQueryAsyncThirdBatchDescending()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                var result = connection.BatchQueryAsync<SdsCompleteTable>(2,
                    3,
                    OrderField.Descending<SdsCompleteTable>(c => c.Id).AsEnumerable(),
                    (object)null).Result;

                // Assert
                Helper.AssertPropertiesEquality(tables.ElementAt(3), result.ElementAt(0));
                Helper.AssertPropertiesEquality(tables.ElementAt(1), result.ElementAt(2));
            }
        }

        [TestMethod, ExpectedException(typeof(AggregateException))]
        public void ThrowExceptionOnSqLiteConnectionBatchQueryAsyncWithHints()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                connection.BatchQueryAsync<SdsCompleteTable>(0,
                    3,
                    OrderField.Ascending<SdsCompleteTable>(c => c.Id).AsEnumerable(),
                    (object)null,
                    hints: "WhatEver").Wait();
            }
        }

        #endregion

        #endregion

        #region TableName

        #region Sync

        [TestMethod]
        public void TestSqLiteConnectionBatchQueryViaTableNameFirstBatchAscending()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                var result = connection.BatchQuery(ClassMappedNameCache.Get<SdsCompleteTable>(),
                    0,
                    3,
                    OrderField.Ascending<SdsCompleteTable>(c => c.Id).AsEnumerable(),
                    (object)null);

                // Assert
                Helper.AssertMembersEquality(tables.ElementAt(0), result.ElementAt(0));
                Helper.AssertMembersEquality(tables.ElementAt(2), result.ElementAt(2));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionBatchQueryViaTableNameFirstBatchDescending()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                var result = connection.BatchQuery(ClassMappedNameCache.Get<SdsCompleteTable>(),
                    0,
                    3,
                    OrderField.Descending<SdsCompleteTable>(c => c.Id).AsEnumerable(),
                    (object)null);

                // Assert
                Helper.AssertMembersEquality(tables.ElementAt(9), result.ElementAt(0));
                Helper.AssertMembersEquality(tables.ElementAt(7), result.ElementAt(2));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionBatchQueryViaTableNameThirdBatchAscending()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                var result = connection.BatchQuery(ClassMappedNameCache.Get<SdsCompleteTable>(),
                    2,
                    3,
                    OrderField.Ascending<SdsCompleteTable>(c => c.Id).AsEnumerable(),
                    (object)null);

                // Assert
                Helper.AssertMembersEquality(tables.ElementAt(6), result.ElementAt(0));
                Helper.AssertMembersEquality(tables.ElementAt(8), result.ElementAt(2));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionBatchQueryViaTableNameThirdBatchDescending()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                var result = connection.BatchQuery(ClassMappedNameCache.Get<SdsCompleteTable>(),
                    2,
                    3,
                    OrderField.Descending<SdsCompleteTable>(c => c.Id).AsEnumerable(),
                    (object)null);

                // Assert
                Helper.AssertMembersEquality(tables.ElementAt(3), result.ElementAt(0));
                Helper.AssertMembersEquality(tables.ElementAt(1), result.ElementAt(2));
            }
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void ThrowExceptionOnSqLiteConnectionBatchQueryViaTableNameWithHints()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                connection.BatchQuery(ClassMappedNameCache.Get<SdsCompleteTable>(),
                    0,
                    3,
                    OrderField.Ascending<SdsCompleteTable>(c => c.Id).AsEnumerable(),
                    (object)null,
                    hints: "WhatEver");
            }
        }

        #endregion

        #region Async

        [TestMethod]
        public void TestSqLiteConnectionBatchQueryViaTableNameAsyncFirstBatchAscending()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                var result = connection.BatchQueryAsync(ClassMappedNameCache.Get<SdsCompleteTable>(),
                    0,
                    3,
                    OrderField.Ascending<SdsCompleteTable>(c => c.Id).AsEnumerable(),
                    (object)null).Result;

                // Assert
                Helper.AssertMembersEquality(tables.ElementAt(0), result.ElementAt(0));
                Helper.AssertMembersEquality(tables.ElementAt(2), result.ElementAt(2));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionBatchQueryViaTableNameAsyncFirstBatchDescending()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                var result = connection.BatchQueryAsync(ClassMappedNameCache.Get<SdsCompleteTable>(),
                    0,
                    3,
                    OrderField.Descending<SdsCompleteTable>(c => c.Id).AsEnumerable(),
                    (object)null).Result;

                // Assert
                Helper.AssertMembersEquality(tables.ElementAt(9), result.ElementAt(0));
                Helper.AssertMembersEquality(tables.ElementAt(7), result.ElementAt(2));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionBatchQueryViaTableNameAsyncThirdBatchAscending()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                var result = connection.BatchQueryAsync(ClassMappedNameCache.Get<SdsCompleteTable>(),
                    2,
                    3,
                    OrderField.Ascending<SdsCompleteTable>(c => c.Id).AsEnumerable(),
                    (object)null).Result;

                // Assert
                Helper.AssertMembersEquality(tables.ElementAt(6), result.ElementAt(0));
                Helper.AssertMembersEquality(tables.ElementAt(8), result.ElementAt(2));
            }
        }

        [TestMethod]
        public void TestSqLiteConnectionBatchQueryViaTableNameAsyncThirdBatchDescending()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                var result = connection.BatchQueryAsync(ClassMappedNameCache.Get<SdsCompleteTable>(),
                    2,
                    3,
                    OrderField.Descending<SdsCompleteTable>(c => c.Id).AsEnumerable(),
                    (object)null).Result;

                // Assert
                Helper.AssertMembersEquality(tables.ElementAt(3), result.ElementAt(0));
                Helper.AssertMembersEquality(tables.ElementAt(1), result.ElementAt(2));
            }
        }

        [TestMethod, ExpectedException(typeof(AggregateException))]
        public void ThrowExceptionOnSqLiteConnectionBatchQueryAsyncViaTableNameWithHints()
        {
            using (var connection = new SQLiteConnection(Database.ConnectionStringSDS))
            {
                // Setup
                var tables = Database.CreateSdsCompleteTables(10, connection);

                // Act
                connection.BatchQueryAsync(ClassMappedNameCache.Get<SdsCompleteTable>(),
                    0,
                    3,
                    OrderField.Ascending<SdsCompleteTable>(c => c.Id).AsEnumerable(),
                    (object)null,
                    hints: "WhatEver").Wait();
            }
        }

        #endregion

        #endregion
    }
}
