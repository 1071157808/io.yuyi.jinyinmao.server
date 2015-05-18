// FileInformation: nyanya/Cat.Domain.Users/User.cs
// CreatedTime: 2015/03/05   9:54 PM
// LastUpdatedTime: 2015/03/08   5:30 PM

using Cat.Domain.Users.Database;
using Domian.Events;
using Domian.Models;
using ServiceStack;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Cat.Domain.Users.Models
{
    public partial class User : IAggregateRoot, IHasMemento
    {
        public ICollection<BankCard> BankCards { get; set; }

        public string Cellphone { get; set; }

        public long ClientType { get; set; }

        public long ContractId { get; set; }

        public long FlgMoreI1 { get; set; }

        public long FlgMoreI2 { get; set; }

        public string FlgMoreS1 { get; set; }

        public string FlgMoreS2 { get; set; }

        public string IdentificationCode { get; set; }

        public string InviteBy { get; set; }

        public string IpReg { get; set; }

        public byte[] RowVersion { get; set; }

        public string UserIdentifier { get; set; }

        public UserLoginInfo UserLoginInfo { get; set; }

        public UserPaymentInfo UserPaymentInfo { get; set; }

        public int UserType { get; set; }

        public YLUserInfo YLUserInfo { get; set; }

        public YSBUserInfo YSBUserInfo { get; set; }

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
