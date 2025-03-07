﻿using Npgsql;
using RepoDb.DbHelpers;
using RepoDb.DbSettings;
using RepoDb.StatementBuilders;

namespace RepoDb
{
    /// <summary>
    /// A class used to initialize necessary objects that is connected to <see cref="NpgsqlConnection"/> object.
    /// </summary>
    public static class PostgreSqlBootstrap
    {
        #region Properties

        /// <summary>
        /// Gets the value indicating whether the initialization is completed.
        /// </summary>
        public static bool IsInitialized { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes all necessary settings for PostgreSql.
        /// </summary>
        public static void Initialize()
        {
            // Skip if already initialized
            if (IsInitialized == true)
            {
                return;
            }

            // Map the DbSetting
            DbSettingMapper.Add<NpgsqlConnection>(new PostgreSqlDbSetting(), true);

            // Map the DbHelper
            DbHelperMapper.Add<NpgsqlConnection>(new PostgreSqlDbHelper(), true);

            // Map the Statement Builder
            StatementBuilderMapper.Add<NpgsqlConnection>(new PostgreSqlStatementBuilder(), true);

            // Set the flag
            IsInitialized = true;
        }

        #endregion
    }
}
