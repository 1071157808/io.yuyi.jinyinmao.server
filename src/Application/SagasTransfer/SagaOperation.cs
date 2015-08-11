// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : SagaOperation.cs
// Created          : 2015-08-11  4:31 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-11  4:39 PM
// ***********************************************************************
// <copyright file="SagaOperation.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace SagasTransfer
{
    internal class SagaOperation : Base
    {
        public override async Task SaveToFileAsync(CloudTable table, string path, string tableName)
        {
            await Task.Delay(1);
        }

        public override async Task TransferAsync(string sourceName, string targetName)
        {
            await Task.Delay(1);
        }
    }
}