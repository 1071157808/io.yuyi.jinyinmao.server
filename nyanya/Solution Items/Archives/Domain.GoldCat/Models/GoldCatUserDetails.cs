using System;

namespace Domain.GoldCat.Models
{
    public class GoldCatUserDetails
    {
        public virtual string Answer { get; set; }

        public virtual int ErrorAnswerCount { get; set; }

        public virtual int ErrorSignInCount { get; set; }

        public virtual int Id { get; set; }

        public virtual DateTime LastFailSignInAt { get; set; }

        public virtual DateTime LastSignInAt { get; set; }

        public virtual string LastSignInIp { get; set; }

        public virtual string Question { get; set; }

        public virtual DateTime UpdatedAt { get; set; }

        public virtual int UserId { get; set; }
    }
}