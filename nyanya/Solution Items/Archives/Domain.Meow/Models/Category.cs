// FileInformation: nyanya/Domain.Meow/Category.cs
// CreatedTime: 2014/04/22   5:00 PM
// LastUpdatedTime: 2014/05/07   10:08 AM

using System.Collections.Generic;

namespace Domain.Meow.Models
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