// FileInformation: nyanya/Cqrs.Domain.User/ITokenDigestProvider.cs
// CreatedTime: 2014/07/16   12:27 PM
// LastUpdatedTime: 2014/07/16   4:54 PM

namespace Xingye.Domain.Users.Services.Interfaces
{
    public interface ITokenDigestProvider
    {
        string GetDigestForUser(string name);
    }
}