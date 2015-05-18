using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Item.Models
{
    public class Category
    {
        public virtual string CategoryDescription { get; set; }

        public virtual string CategoryTitle { get; set; }

        public virtual int Id { get; set; }

        public virtual string ImageSrc { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
