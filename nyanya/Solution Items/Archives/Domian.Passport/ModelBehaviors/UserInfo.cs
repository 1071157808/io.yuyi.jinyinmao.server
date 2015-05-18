// FileInformation: nyanya/Domian.Passport/UserInfo.cs
// CreatedTime: 2014/04/01   2:10 PM
// LastUpdatedTime: 2014/04/22   1:31 PM

namespace Domain.Passport.Models
{
    public partial class UserInfo
    {
        public string GetIdCardWithMosaic()
        {
            return this.IdCard == null ? "" : this.IdCard.Substring(0, 4) + "**********" + this.IdCard.Substring(this.IdCard.Length - 4, 4);
        }
    }
}