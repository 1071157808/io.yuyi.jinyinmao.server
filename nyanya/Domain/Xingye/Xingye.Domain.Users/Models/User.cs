// FileInformation: nyanya/Cqrs.Domain.User/User.cs
// CreatedTime: 2014/07/16   5:00 PM
// LastUpdatedTime: 2014/07/20   7:24 PM

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Xingye.Domain.Users.Database;
using Domian.Events;
using Domian.Models;
using ServiceStack;

namespace Xingye.Domain.Users.Models
{
    public partial class User : IAggregateRoot, IHasMemento
    {
        public ICollection<BankCard> BankCards { get; set; }

        public string Cellphone { get; set; }

        public string IdentificationCode { get; set; }

        public byte[] RowVersion { get; set; }

        public string UserIdentifier { get; set; }

        public UserLoginInfo UserLoginInfo { get; set; }

        public UserPaymentInfo UserPaymentInfo { get; set; }

        public YLUserInfo YLUserInfo { get; set; }

        public YSBUserInfo YSBUserInfo { get; set; }

        /// <summary>
        /// 用户类型（10金银猫 20兴业）
        /// </summary>
        public int UserType { get; set; }

        #region IHasMemento Members

        public AggregateMemento GetMemento()
        {
            User user;
            using (UserContext context = new UserContext())
            {
                user = context.ReadonlyQuery<User>()
                    .Include(u => u.UserLoginInfo)
                    .Include(u => u.UserPaymentInfo)
                    .Include(u => u.YLUserInfo)
                    .Include(u => u.YSBUserInfo)
                    .Include(u => u.BankCards)
                    .FirstOrDefault(u => u.UserIdentifier == this.UserIdentifier);
            }

            return new AggregateMemento
            {
                Value = user == null ? "" : user.ToJson()
            };
        }

        #endregion IHasMemento Members
    }
}