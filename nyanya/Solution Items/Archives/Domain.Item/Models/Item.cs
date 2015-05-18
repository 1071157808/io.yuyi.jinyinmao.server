using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Item.Models
{
    public partial class Item
    {
        public virtual Category Category { get; set; }

        public virtual int CategoryId { get; set; }

        public virtual DateTime Expires { get; set; }

        public virtual int Id { get; set; }

        public virtual bool IsUsed { get; set; }

        public virtual string OwnerGuid { get; set; }

        public virtual DateTime ReceiveTime { get; set; }

        public virtual Nullable<DateTime> UseTime { get; set; }
    }
}
