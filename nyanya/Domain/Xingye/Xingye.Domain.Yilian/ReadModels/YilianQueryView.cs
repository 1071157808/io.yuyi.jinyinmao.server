using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xingye.Domain.Yilian.ReadModels
{
    public class YilianQueryView
    {
        public int Id { get; set; }

        public string RequestIdentifier { get; set; }

        public DateTime CreateTime { get; set; }

        public bool? GatewayResult { get; set; }
        
        public bool? CallbackResult { get; set; }

        public bool? QueryResult { get; set; }

        public string SequenceNo { get; set; }

        public string UserIdentifier { get; set; }
        public string OrderIdentifier { get; set; }
        public bool IsPayment { get; set; }
    }
}
