// FileInformation: nyanya/Xingye.Events.Users/IAddedABankCard.cs
// CreatedTime: 2014/09/02   10:16 AM
// LastUpdatedTime: 2014/09/02   10:43 AM

namespace Xingye.Events.Users
{
    public interface IAddedABankCard
    {
        string BankCardNo { get; }

        string BankName { get; }

        string Cellphone { get; }

        string UserIdentifier { get; }
    }
}