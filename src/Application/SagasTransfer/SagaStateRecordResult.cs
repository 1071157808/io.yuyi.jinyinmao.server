using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagasTransfer
{
    public class SagaStateRecordResult
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTime BeginTime { get; set; }
        public int CurrentProcessingStatus { get; set; }
        public string Info { get; set; }
        public string Message { get; set; }
        public Guid SagaId { get; set; }
        public string SagaState { get; set; }
        public string SagaType { get; set; }
        public int State { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
