using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
using Serilog;
using Yuyi.Jinyinmao.Domain;

namespace SagasTransfer
{
    class SagaOperation : Base
    {
        public override async Task TransferAsync(string sourceName, string targetName)
        {
            await Task.Delay(1);
        }

        public override async Task SaveToFileAsync(CloudTable table,string path, string tableName)
        {
            await Task.Delay(1);
        }
    }
}
