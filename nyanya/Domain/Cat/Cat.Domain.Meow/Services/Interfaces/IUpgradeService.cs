// FileInformation: nyanya/Cat.Domain.Meow/IFeedbackService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:29 PM

using System.Threading.Tasks;
using Cat.Domain.Meow.Services.DTO;
using Domian.Models;

namespace Cat.Domain.Meow.Services.Interfaces
{
    public interface IUpgradeService : IDomainService
    {
        Task<UpgradeResult> GetUpgradeAsync(string channel, string source, string version);

        Task<UpgradeExResult> GetUpgradeExAsync(string channel, string source, string version);
    }
}