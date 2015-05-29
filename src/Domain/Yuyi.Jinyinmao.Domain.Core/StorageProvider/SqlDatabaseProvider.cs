// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:36 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-29  2:31 PM
// ***********************************************************************
// <copyright file="SqlDatabaseProvider.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Moe.Lib;
using Newtonsoft.Json;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using Orleans.Storage;
using SqlMapper = Dapper.SqlMapper;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Class SqlDatabaseProvider.
    /// </summary>
    public class SqlDatabaseProvider : IStorageProvider
    {
        private const int DBCOUNT = 6;
        private const int TABLECOUNT = 9;
        private static readonly string InsertCommandString;
        private static readonly string Letters;
        private static readonly string SelectCommandString;
#pragma warning disable 414
        private static readonly string UpdateCommandString;
#pragma warning restore 414
        private string connectionStringTemplate;
        private RetryPolicy retryPolicy;
        private JsonSerializerSettings settings;
        private string tableNameTemplate;

        static SqlDatabaseProvider()
        {
            Letters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            InsertCommandString = "INSERT INTO [dbo].[{0}] ([Id], [LongId], [Key], [Type], [TimeStamp], [Data]) VALUES(@Id, @LongId, @Key, @Type, @TimeStamp, @Data)";
            SelectCommandString = "SELECT TOP 1 * FROM [dbo].[{0}] WHERE [Key] = @Key ORDER BY [TimeStamp] DESC";
            UpdateCommandString = "UPDATE TOP(1) [dbo].[{0}] SET [Id]=@Id, [LongId]=@LongId, [Key]=@Key, [Type]=@Type, [TimeStamp]=@TimeStamp, [Data]=@Data WHERE [Key]=@Key AND [TimeStamp]=@Etag";
        }

        #region IStorageProvider Members

        /// <summary>
        ///     Logger of this storage provider instance.
        /// </summary>
        public Logger Log { get; private set; }

        /// <summary>
        ///     Name of this storage provider instance.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Clears the state asynchronous. Do nothing.
        /// </summary>
        /// <param name="grainType">Type of the grain.</param>
        /// <param name="grainReference">The grain reference.</param>
        /// <param name="grainState">State of the grain.</param>
        /// <returns>Task.</returns>
        public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState) => TaskDone.Done;

        /// <summary>
        ///     Closes this instance.
        /// </summary>
        /// <returns>Task.</returns>
        public Task Close() => TaskDone.Done;

        /// <summary>
        ///     Initialization function for this storage provider.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="providerRuntime">The provider runtime.</param>
        /// <param name="config">The configuration.</param>
        /// <returns>Task.</returns>
        /// <exception cref="BadProviderConfigException">The DataConnectionString setting has not been configured. Please add a DataConnectionString setting with a valid connection string.</exception>
        [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
        public async Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
        {
            try
            {
                this.Log = providerRuntime.GetLogger(this.GetType().Name);
                this.Name = name;
                this.retryPolicy = RetryPolicyHelper.SqlDatabase;

                this.ConfigureJsonSerializerSettings(config);

                string connectionString = CloudConfigurationManager.GetSetting("StorageProviderConnectionString");

                if (connectionString.IsNullOrEmpty())
                {
                    throw new BadProviderConfigException("The DataConnectionString setting has not been configured. Please add a DataConnectionString setting with a valid connection string.");
                }
                this.connectionStringTemplate = connectionString;

                string tableName;
                this.tableNameTemplate = config.Properties.TryGetValue("TableName", out tableName) ? tableName : "Grains_{0}";

                using (IDbConnection db = new SqlConnection(this.connectionStringTemplate.FormatWith(0)))
                {
                    await this.retryPolicy.ExecuteAsync(async () => await SqlMapper.QueryAsync<int>(db, "SELECT count(Id) FROM " + this.tableNameTemplate.FormatWith(0)));
                }
            }
            catch (Exception e)
            {
                this.Log.Error((int)ErrorCode.SqlDatabaseProviderInitProvider, e.Message, e);
            }
        }

        /// <summary>
        ///     Read state data function for this storage provider.
        /// </summary>
        /// <param name="grainType">Type of the grain.</param>
        /// <param name="grainReference">The grain reference.</param>
        /// <param name="grainState">State of the grain.</param>
        /// <returns>Task.</returns>
        [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
        public async Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            try
            {
                string connectionString = this.GetConnectionString(grainReference);
                string tableName = this.GetTableName(grainReference);

                this.Log.Verbose("SqlDatabaseProvider:{0}-{1}", connectionString, tableName);

                GrainStateRecord record;
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    record = (await this.retryPolicy.ExecuteAsync(async () =>
                        await SqlMapper.QueryAsync<GrainStateRecord>(db, SelectCommandString.FormatWith(tableName),
                            new { Key = grainReference.ToKeyString() }))).FirstOrDefault();
                }

                if (record != null && record.Data.IsNotNullOrEmpty())
                {
                    object data = JsonConvert.DeserializeObject(record.Data, grainState.GetType(), this.settings);
                    IDictionary<string, object> dict = ((IGrainState)data).AsDictionary();
                    grainState.SetAll(dict);
                    grainState.Etag = record.TimeStamp.ToString();
                }
                else
                {
                    grainState.Etag = "0";
                }
                // Else leave grainState in previous default condition
            }
            catch (Exception e)
            {
                this.Log.Error((int)ErrorCode.SqlDatabaseProviderRead, "{0} : {1}".FormatWith(e.Message, grainReference.GetPrimaryKey().ToGuidString()), e);
                throw;
            }
        }

        /// <summary>
        ///     Writes the state asynchronous.
        /// </summary>
        /// <param name="grainType">Type of the grain.</param>
        /// <param name="grainReference">The grain reference.</param>
        /// <param name="grainState">State of the grain.</param>
        /// <returns>Task.</returns>
        /// <exception cref="System.ApplicationException">Id:{0},Key{2} can not write the grain state.</exception>
        [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
        public async Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            string key = grainReference.ToKeyString();
            string id = grainReference.GetPrimaryKey().ToGuidString();
            long longId = 0;

            try
            {
                IDictionary<string, object> grainStateDictionary = grainState.AsDictionary();
                string storedData = JsonConvert.SerializeObject(grainStateDictionary, this.settings);
                long timeStamp = DateTime.UtcNow.Ticks;
                object timeStampState;
                if (grainStateDictionary.TryGetValue("TimeStamp", out timeStampState))
                {
                    timeStamp = Convert.ToInt64(timeStampState);
                }
                string connectionString = this.GetConnectionString(grainReference);
                string tableName = this.GetTableName(grainReference);

                this.Log.Verbose("SqlDatabaseProvider:{0}-{1}-{2}", connectionString, tableName, storedData);

                // Grain use the long as its key
                if (id.StartsWith("00000000", StringComparison.Ordinal) && !id.EndsWith("000000", StringComparison.Ordinal))
                {
                    longId = grainReference.GetPrimaryKeyLong();
                }

                int resultCount;

                await Task.Run(async () =>
                {
                    using (IDbConnection db = new SqlConnection(connectionString))
                    {
                        resultCount = await this.retryPolicy.ExecuteAsync(async () => await SqlMapper.ExecuteAsync(db, InsertCommandString.FormatWith(tableName), new
                        {
                            Id = id,
                            LongId = longId,
                            Key = key,
                            Type = grainType,
                            TimeStamp = timeStamp,
                            Data = storedData
                        }));
                    }

                    if (resultCount == 0)
                    {
                        throw new ApplicationException("Grain Write State Failed. {0}".FormatWith(id));
                    }
                });

                //if (grainState.Etag.IsNullOrEmpty() || grainState.Etag == "0")
                //{
                //    using (IDbConnection db = new SqlConnection(connectionString))
                //    {
                //        resultCount = await this.retryPolicy.ExecuteAsync(async () => await SqlMapper.ExecuteAsync(db, InsertCommandString.FormatWith(tableName), new
                //        {
                //            Id = id,
                //            LongId = longId,
                //            Key = key,
                //            Type = grainType,
                //            TimeStamp = timeStamp,
                //            Data = storedData
                //        }));
                //    }
                //}
                //else
                //{
                //    using (IDbConnection db = new SqlConnection(connectionString))
                //    {
                //        resultCount = await this.retryPolicy.ExecuteAsync(async () => await SqlMapper.ExecuteAsync(db, UpdateCommandString.FormatWith(tableName), new
                //        {
                //            Id = id,
                //            LongId = longId,
                //            Key = key,
                //            Type = grainType,
                //            TimeStamp = timeStamp,
                //            Data = storedData,
                //            grainState.Etag
                //        }));
                //    }
                //}

                grainState.Etag = timeStamp.ToString();
            }
            catch (Exception e)
            {
                this.Log.Error((int)ErrorCode.SqlDatabaseProviderWrite, "{0} : {1}".FormatWith(e.Message, id), e);
            }
        }

        #endregion IStorageProvider Members

        /// <summary>
        ///     Configures the json serializer settings.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private void ConfigureJsonSerializerSettings(IProviderConfiguration config)
        {
            // By default, use automatic type name handling, simple assembly names, and no JSON formatting
            this.settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                Formatting = Formatting.None
            };

            string propertie;
            if (config.Properties.TryGetValue("SerializeTypeNames", out propertie))
            {
                bool serializeTypeNames;
                var serializeTypeNamesValue = propertie;
                bool.TryParse(serializeTypeNamesValue, out serializeTypeNames);
                if (serializeTypeNames)
                {
                    this.settings.TypeNameHandling = TypeNameHandling.All;
                }
            }

            if (config.Properties.TryGetValue("UseFullAssemblyNames", out propertie))
            {
                bool useFullAssemblyNames;
                var useFullAssemblyNamesValue = propertie;
                bool.TryParse(useFullAssemblyNamesValue, out useFullAssemblyNames);
                if (useFullAssemblyNames)
                {
                    this.settings.TypeNameAssemblyFormat = FormatterAssemblyStyle.Full;
                }
            }

            if (config.Properties.TryGetValue("Formatting", out propertie))
            {
                bool indentJson;
                var indentJsonValue = propertie;
                bool.TryParse(indentJsonValue, out indentJson);
                if (indentJson)
                {
                    this.settings.Formatting = Formatting.Indented;
                }
            }
        }

        /// <summary>
        ///     Gets the connection string.
        /// </summary>
        /// <param name="grainReference">The grain reference.</param>
        /// <returns>System.String.</returns>
        private string GetConnectionString(GrainReference grainReference)
        {
            string id = grainReference.GetPrimaryKey().ToString();
            char lastLetter = char.ToUpperInvariant(id[35]);
            return this.connectionStringTemplate.FormatWith(Letters.IndexOf(lastLetter) % DBCOUNT);
        }

        /// <summary>
        ///     Gets the name of the table.
        /// </summary>
        /// <param name="grainReference">The grain reference.</param>
        /// <returns>System.String.</returns>
        private string GetTableName(GrainReference grainReference)
        {
            string id = grainReference.GetPrimaryKey().ToString();
            char penultimateLetter = char.ToUpperInvariant(id[34]);
            return this.tableNameTemplate.FormatWith(Letters.IndexOf(penultimateLetter) % TABLECOUNT);
        }
    }

    /// <summary>
    ///     Class GrainStateRecord.
    /// </summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global"), SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    internal class GrainStateRecord
    {
        /// <summary>
        ///     Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public string Data { get; set; }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }

        /// <summary>
        ///     Gets or sets the long identifier.
        /// </summary>
        /// <value>The long identifier.</value>
        public long LongId { get; set; }

        /// <summary>
        ///     Gets or sets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        public long TimeStamp { get; set; }

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }
    }
}