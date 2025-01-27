﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RepoDb.Resolvers;
using System;
using System.Data.SQLite;

namespace RepoDb.SQLite.System.UnitTests.Resolvers
{
    [TestClass]
    public class SqLiteConvertFieldResolverTest
    {
        [TestInitialize]
        public void Initialize()
        {
            SQLiteBootstrap.Initialize();
        }

        #region SDS

        [TestMethod]
        public void TestSdsSqLiteConvertFieldResolverForInt32()
        {
            // Setup
            var setting = DbSettingMapper.Get<SQLiteConnection>();
            var resolver = new SqLiteConvertFieldResolver();
            var field = new Field("Field", typeof(int));

            // Act
            var result = resolver.Resolve(field, setting);

            // Assert
            Assert.AreEqual("CAST([Field] AS [INT])", result);
        }

        [TestMethod]
        public void TestSdsSqLiteConvertFieldResolverForInt64()
        {
            // Setup
            var setting = DbSettingMapper.Get<SQLiteConnection>();
            var resolver = new SqLiteConvertFieldResolver();
            var field = new Field("Field", typeof(long));

            // Act
            var result = resolver.Resolve(field, setting);

            // Assert
            Assert.AreEqual("CAST([Field] AS [BIGINT])", result);
        }

        [TestMethod]
        public void TestSdsSqLiteConvertFieldResolverForInt16()
        {
            // Setup
            var setting = DbSettingMapper.Get<SQLiteConnection>();
            var resolver = new SqLiteConvertFieldResolver();
            var field = new Field("Field", typeof(short));

            // Act
            var result = resolver.Resolve(field, setting);

            // Assert
            Assert.AreEqual("CAST([Field] AS [INT])", result);
        }

        [TestMethod]
        public void TestSdsSqLiteConvertFieldResolverForDateTime()
        {
            // Setup
            var setting = DbSettingMapper.Get<SQLiteConnection>();
            var resolver = new SqLiteConvertFieldResolver();
            var field = new Field("Field", typeof(DateTime));

            // Act
            var result = resolver.Resolve(field, setting);

            // Assert
            Assert.AreEqual("CAST([Field] AS [DATETIME])", result);
        }

        [TestMethod]
        public void TestSdsSqLiteConvertFieldResolverForString()
        {
            // Setup
            var setting = DbSettingMapper.Get<SQLiteConnection>();
            var resolver = new SqLiteConvertFieldResolver();
            var field = new Field("Field", typeof(string));

            // Act
            var result = resolver.Resolve(field, setting);

            // Assert
            Assert.AreEqual("CAST([Field] AS [TEXT])", result);
        }

        [TestMethod]
        public void TestSdsSqLiteConvertFieldResolverForByte()
        {
            // Setup
            var setting = DbSettingMapper.Get<SQLiteConnection>();
            var resolver = new SqLiteConvertFieldResolver();
            var field = new Field("Field", typeof(byte));

            // Act
            var result = resolver.Resolve(field, setting);

            // Assert
            Assert.AreEqual("CAST([Field] AS [BLOB])", result);
        }

        [TestMethod]
        public void TestSdsSqLiteConvertFieldResolverForDecimal()
        {
            // Setup
            var setting = DbSettingMapper.Get<SQLiteConnection>();
            var resolver = new SqLiteConvertFieldResolver();
            var field = new Field("Field", typeof(decimal));

            // Act
            var result = resolver.Resolve(field, setting);

            // Assert
            Assert.AreEqual("CAST([Field] AS [DECIMAL])", result);
        }

        [TestMethod]
        public void TestSdsSqLiteConvertFieldResolverForFloat()
        {
            // Setup
            var setting = DbSettingMapper.Get<SQLiteConnection>();
            var resolver = new SqLiteConvertFieldResolver();
            var field = new Field("Field", typeof(float));

            // Act
            var result = resolver.Resolve(field, setting);

            // Assert
            Assert.AreEqual("CAST([Field] AS [REAL])", result);
        }

        [TestMethod]
        public void TestSdsSqLiteConvertFieldResolverForTimeSpan()
        {
            // Setup
            var setting = DbSettingMapper.Get<SQLiteConnection>();
            var resolver = new SqLiteConvertFieldResolver();
            var field = new Field("Field", typeof(TimeSpan));

            // Act
            var result = resolver.Resolve(field, setting);

            // Assert
            Assert.AreEqual("CAST([Field] AS [TIME])", result);
        }

        #endregion
    }
}
