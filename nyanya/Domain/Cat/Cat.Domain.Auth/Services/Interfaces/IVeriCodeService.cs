// FileInformation: nyanya/Cat.Domain.Auth/IVeriCodeService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:29 PM

using System.Threading.Tasks;
using Cat.Domain.Auth.Models;
using Cat.Domain.Auth.Services.DTO;
using Domian.Models;

namespace Cat.Domain.Auth.Services.Interfaces
{
    public interface IVeriCodeService : IDomainService
    {
        Task<SendVeriCodeResult> SendAsync(string cellphone, VeriCode.VeriCodeType type);

        Task<UseVeriCodeResult> UseAsync(string code, VeriCode.VeriCodeType type);

        Task<VerifyVeriCodeResult> VerifyAsync(string cellphone, string code, VeriCode.VeriCodeType type);

        Task<SendVeriCodePhoneResult> SendGraphicAsync(string identifier, int imageWidth, string clientId = "");

        Task<VerifyVeriCodeResult> VerifyGraphicAsync(string cellphone, string identifier, string addResult, string ClientId = "");

        Task<SendVeriCodeResult> SendWithTokenAsync(string cellphone, string identifier, VeriCode.VeriCodeType type);
    }
}