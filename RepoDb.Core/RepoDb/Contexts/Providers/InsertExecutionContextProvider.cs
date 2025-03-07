﻿using RepoDb.Contexts.Cachers;
using RepoDb.Contexts.Execution;
using RepoDb.Extensions;
using RepoDb.Interfaces;
using RepoDb.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RepoDb.Contexts.Providers
{
    /// <summary>
    /// 
    /// </summary>
    internal static class InsertExecutionContextProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="hints"></param>
        /// <returns></returns>
        private static string GetKey(Type type,
            string tableName,
            IEnumerable<Field> fields,
            string hints)
        {
            return string.Concat(type.FullName,
                ";",
                tableName,
                ";",
                fields?.Select(f => f.Name).Join(","),
                ";",
                hints);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="hints"></param>
        /// <param name="transaction"></param>
        /// <param name="statementBuilder"></param>
        /// <returns></returns>
        public static InsertExecutionContext Create(Type entityType,
            IDbConnection connection,
            string tableName,
            IEnumerable<Field> fields,
            string hints = null,
            IDbTransaction transaction = null,
            IStatementBuilder statementBuilder = null)
        {
            var key = GetKey(entityType, tableName, fields, hints);

            // Get from cache
            var context = InsertExecutionContextCache.Get(key);
            if (context != null)
            {
                return context;
            }

            // Create
            var dbFields = DbFieldCache.Get(connection, tableName, transaction);
            var request = new InsertRequest(tableName,
                connection,
                transaction,
                fields,
                hints,
                statementBuilder);
            var commandText = CommandTextCache.GetInsertText(request);

            // Call
            context = CreateInternal(entityType, connection,
                dbFields,
                tableName,
                fields,
                commandText);

            // Add to cache
            InsertExecutionContextCache.Add(entityType, key, context);

            // Return
            return context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="hints"></param>
        /// <param name="transaction"></param>
        /// <param name="statementBuilder"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<InsertExecutionContext> CreateAsync(Type entityType,
            IDbConnection connection,
            string tableName,
            IEnumerable<Field> fields,
            string hints = null,
            IDbTransaction transaction = null,
            IStatementBuilder statementBuilder = null,
            CancellationToken cancellationToken = default)
        {
            var key = GetKey(entityType, tableName, fields, hints);

            // Get from cache
            var context = InsertExecutionContextCache.Get(key);
            if (context != null)
            {
                return context;
            }

            // Create
            var dbFields = await DbFieldCache.GetAsync(connection, tableName, transaction, cancellationToken);
            var request = new InsertRequest(tableName,
                connection,
                transaction,
                fields,
                hints,
                statementBuilder);
            var commandText = await CommandTextCache.GetInsertTextAsync(request, cancellationToken);

            // Call
            context = CreateInternal(entityType,
                connection,
                dbFields,
                tableName,
                fields,
                commandText);

            // Add to cache
            InsertExecutionContextCache.Add(entityType, key, context);

            // Return
            return context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="connection"></param>
        /// <param name="dbFields"></param>
        /// <param name="tableName"></param>
        /// <param name="fields"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        private static InsertExecutionContext CreateInternal(Type entityType,
            IDbConnection connection,
            IEnumerable<DbField> dbFields,
            string tableName,
            IEnumerable<Field> fields,
            string commandText)
        {
            var dbSetting = connection.GetDbSetting();
            var identity = (Field)null;
            var inputFields = (IEnumerable<DbField>)null;
            var identityDbField = dbFields?.FirstOrDefault(f => f.IsIdentity);

            // Set the primary field
            identity = IdentityCache.Get(entityType)?.AsField() ??
                FieldCache
                    .Get(entityType)?
                    .FirstOrDefault(field =>
                        string.Equals(field.Name.AsUnquoted(true, dbSetting), identityDbField?.Name.AsUnquoted(true, dbSetting), StringComparison.OrdinalIgnoreCase)) ??
                identityDbField?.AsField();

            // Filter the actual properties for input fields
            inputFields = dbFields?
                .Where(dbField =>
                    dbField.IsIdentity == false)
                .Where(dbField =>
                    fields.FirstOrDefault(field =>
                        string.Equals(field.Name.AsUnquoted(true, dbSetting), dbField.Name.AsUnquoted(true, dbSetting), StringComparison.OrdinalIgnoreCase)) != null)
                .AsList();

            // Variables for the entity action
            Action<object, object> identityPropertySetterFunc = null;

            // Get the identity setter
            if (identity != null)
            {
                identityPropertySetterFunc = FunctionCache
                    .GetDataEntityPropertySetterCompiledFunction(entityType, identity);
            }

            // Return the value
            return new InsertExecutionContext
            {
                CommandText = commandText,
                InputFields = inputFields,
                ParametersSetterFunc = FunctionCache
                    .GetDataEntityDbParameterSetterCompiledFunction(entityType,
                        string.Concat(entityType.FullName, StringConstant.Period, tableName, ".Insert"),
                        inputFields?.AsList(),
                        null,
                        dbSetting),
                IdentityPropertySetterFunc = identityPropertySetterFunc
            };
        }
    }
}
