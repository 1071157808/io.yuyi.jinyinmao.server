// FileInformation: nyanya/Xingye.Events.Users/IAddingBankCardFailed.cs
// CreatedTime: 2014/09/02   10:16 AM
// LastUpdatedTime: 2014/09/02   10:43 AM

namespace Xingye.Events.Users
{
    public interface IAddingBankCardFailed
    {
        string BankCardNo { get; }

        string BankName { get; }

        string Cellphone { get; }

        string Message { get; }

        string UserIdentifier { get; }
    }
}