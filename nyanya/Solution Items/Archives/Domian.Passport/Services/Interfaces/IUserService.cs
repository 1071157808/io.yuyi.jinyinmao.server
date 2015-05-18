// FileInformation: nyanya/Domian.Passport/IUserService.cs
// CreatedTime: 2014/04/16   10:45 AM
// LastUpdatedTime: 2014/04/21   10:54 PM

using System.Net.Http.Headers;
using System.Threading.Tasks;
using Domain.Passport.Models;

namespace Domian.Passport.Services.Interfaces
{
    public interface IUserService
    {
        Task AddSourceInfo(string name, HttpRequestHeaders httpRequestHeaders);

        Task<User> BuildUser(string name, string password);

        Task ResetPassword(string cellphone, string password);
    }
}