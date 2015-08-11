using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace SagasTransfer
{
    abstract class Base
    {
        public abstract Task TransferAsync(string sourceName, string targetName);
        public abstract Task SaveToFileAsync(CloudTable table,string path, string tableName);
    }
}
