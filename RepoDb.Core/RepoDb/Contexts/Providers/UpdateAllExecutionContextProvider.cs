﻿using RepoDb.Contexts.Cachers;
using RepoDb.Contexts.Execution;
using RepoDb.Extensions;
using RepoDb.Interfaces;
using RepoDb.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RepoDb.Contexts.Providers
{
    /// <summary>
    /// 
    /// </summary>
    internal static class UpdateAllExecutionContextProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="tableName"></param>
        /// <param name="qualifiers"></param>
        /// <param name="fields"></param>
        /// <param name="batchSize"></param>
        /// <param name="hints"></param>
        /// <returns></returns>
        private static string GetKey(Type entityType,
            string tableName,
            IEnumerable<Field> qualifiers,
            IEnumerable<Field> fields,
            int batchSize,
            string hints)
        {
            return string.Concat(entityType.FullName,
                ";",
                tableName,
                ";",
                qualifiers?.Select(f => f.Name).Join(","),
                ";",
                fields?.Select(f => f.Name).Join(","),
                ";",
                batchSize.ToString(),
                ";",
                hints);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <param name="qualifiers"></param>
        /// <param name="batchSize"></param>
        /// <param name="fields"></param>
        /// <param name="hints"></param>
        /// <param name="transaction"></param>
        /// <param name="statementBuilder"></param>
        /// <returns></returns>
        public static UpdateAllExecutionContext Create(Type entityType,
            IDbConnection connection,
            string tableName,
            IEnumerable<object> entities,
            IEnumerable<Field> qualifiers,
            int batchSize,
            IEnumerable<Field> fields,
            string hints = null,
            IDbTransaction transaction = null,
            IStatementBuilder statementBuilder = null)
        {
            var key = GetKey(entityType, tableName, qualifiers, fields, batchSize, hints);

            // Get from cache
            var context = UpdateAllExecutionContextCache.Get(key);
            if (context != null)
            {
                return context;
            }

            // Create
            var dbFields = DbFieldCache.Get(connection, tableName, transaction);
            var request = new UpdateAllRequest(tableName,
                connection,
                transaction,
                fields,
                qualifiers,
                batchSize,
                hints,
                statementBuilder);
            var commandText = CommandTextCache.GetUpdateAllText(request);

            // Call
            context = CreateInternal(entityType,
                connection,
                tableName,
                entities,
                dbFields,
                qualifiers,
                batchSize,
                fields,
                commandText);

            // Add to cache
            UpdateAllExecutionContextCache.Add(key, context);

            // Return
            return context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <param name="qualifiers"></param>
        /// <param name="batchSize"></param>
        /// <param name="fields"></param>
        /// <param name="hints"></param>
        /// <param name="transaction"></param>
        /// <param name="statementBuilder"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<UpdateAllExecutionContext> CreateAsync(Type entityType,
            IDbConnection connection,
            string tableName,
            IEnumerable<object> entities,
            IEnumerable<Field> qualifiers,
            int batchSize,
            IEnumerable<Field> fields,
            string hints = null,
            IDbTransaction transaction = null,
            IStatementBuilder statementBuilder = null,
            CancellationToken cancellationToken = default)
        {
            var key = GetKey(entityType, tableName, qualifiers, fields, batchSize, hints);

            // Get from cache
            var context = UpdateAllExecutionContextCache.Get(key);
            if (context != null)
            {
                return context;
            }

            // Create
            var dbFields = await DbFieldCache.GetAsync(connection, tableName, transaction, cancellationToken);
            var request = new UpdateAllRequest(tableName,
                connection,
                transaction,
                fields,
                qualifiers,
                batchSize,
                hints,
                statementBuilder);
            var commandText = await CommandTextCache.GetUpdateAllTextAsync(request, cancellationToken);

            // Call
            context = CreateInternal(entityType,
                connection,
                tableName,
                entities,
                dbFields,
                qualifiers,
                batchSize,
                fields,
                commandText);

            // Add to cache
            UpdateAllExecutionContextCache.Add(key, context);

            // Return
            return context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <param name="dbFields"></param>
        /// <param name="qualifiers"></param>
        /// <param name="batchSize"></param>
        /// <param name="fields"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        private static UpdateAllExecutionContext CreateInternal(Type entityType,
            IDbConnection connection,
            string tableName,
            IEnumerable<object> entities,
            IEnumerable<DbField> dbFields,
            IEnumerable<Field> qualifiers,
            int batchSize,
            IEnumerable<Field> fields,
            string commandText)
        {
            // Variables needed
            var dbSetting = connection.GetDbSetting();
            var inputFields = new List<DbField>();

            // Check the qualifiers
            if (qualifiers?.Any() != true)
            {
                var key = dbFields?.FirstOrDefault(dbField => dbField.IsPrimary == true) ??
                    dbFields?.FirstOrDefault(dbField => dbField.IsIdentity == true);
                qualifiers = key?.AsField().AsEnumerable();
            }

            // Filter the actual properties for input fields
            inputFields = dbFields?
                .Where(dbField =>
                    fields.FirstOrDefault(field => string.Equals(field.Name.AsUnquoted(true, dbSetting), dbField.Name.AsUnquoted(true, dbSetting), StringComparison.OrdinalIgnoreCase)) != null)
                .AsList();

            // Exclude the fields not on the actual entity
            if (entityType.IsClassType() == false)
            {
                var entityFields = Field.Parse(entities?.FirstOrDefault());
                inputFields = inputFields?
                    .Where(field =>
                        entityFields.FirstOrDefault(f => string.Equals(f.Name.AsUnquoted(true, dbSetting), field.Name.AsUnquoted(true, dbSetting), StringComparison.OrdinalIgnoreCase)) != null)
                    .AsList();
            }

            // Variables for the context
            Action<DbCommand, IList<object>> multipleEntitiesParametersSetterFunc = null;
            Action<DbCommand, object> singleEntityParametersSetterFunc = null;

            // Identity which objects to set
            if (batchSize <= 1)
            {
                singleEntityParametersSetterFunc = FunctionCache.GetDataEntityDbParameterSetterCompiledFunction(entityType,
                    string.Concat(entityType.FullName, StringConstant.Period, tableName, ".UpdateAll"),
                    inputFields,
                    null,
                    dbSetting);
            }
            else
            {
                multipleEntitiesParametersSetterFunc = FunctionCache.GetDataEntityListDbParameterSetterCompiledFunction(entityType,
                    string.Concat(entityType.FullName, StringConstant.Period, tableName, ".UpdateAll"),
                    inputFields,
                    null,
                    batchSize,
                    dbSetting);
            }

            // Return the value
            return new UpdateAllExecutionContext
            {
                CommandText = commandText,
                InputFields = inputFields,
                SingleDataEntityParametersSetterFunc = singleEntityParametersSetterFunc,
                MultipleDataEntitiesParametersSetterFunc = multipleEntitiesParametersSetterFunc
            };
        }
    }
}
