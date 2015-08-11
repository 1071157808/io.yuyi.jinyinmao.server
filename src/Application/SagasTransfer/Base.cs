// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Base.cs
// Created          : 2015-08-11  4:31 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-11  4:33 PM
// ***********************************************************************
// <copyright file="Base.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace SagasTransfer
{
    internal abstract class Base
    {
        public abstract Task SaveToFileAsync(CloudTable table, string path, string tableName);

        public abstract Task TransferAsync(string sourceName, string targetName);
    }
}