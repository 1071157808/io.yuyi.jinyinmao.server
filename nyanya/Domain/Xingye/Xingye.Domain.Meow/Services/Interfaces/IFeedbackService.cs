// FileInformation: nyanya/Xingye.Domain.Meow/IFeedbackService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:29 PM

using System.Threading.Tasks;
using Domian.Models;

namespace Xingye.Domain.Meow.Services.Interfaces
{
    public interface IFeedbackService : IDomainService
    {
        Task AddFeedbackAsync(string content, string cellphone = "");
    }
}