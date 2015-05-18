// FileInformation: nyanya/Xingye.Domain.Auth/IVeriCodeService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:29 PM

using System.Threading.Tasks;
using Xingye.Domain.Auth.Models;
using Xingye.Domain.Auth.Services.DTO;
using Domian.Models;

namespace Xingye.Domain.Auth.Services.Interfaces
{
    public interface IVeriCodeService : IDomainService
    {
        Task<SendVeriCodeResult> SendAsync(string cellphone, VeriCode.VeriCodeType type);

        Task<UseVeriCodeResult> UseAsync(string code, VeriCode.VeriCodeType type);

        Task<VerifyVeriCodeResult> VerifyAsync(string cellphone, string code, VeriCode.VeriCodeType type);
    }
}