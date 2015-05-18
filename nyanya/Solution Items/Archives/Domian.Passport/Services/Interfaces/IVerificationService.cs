// FileInformation: nyanya/Domian.Passport/IVerificationService.cs
// CreatedTime: 2014/04/02   4:55 PM
// LastUpdatedTime: 2014/04/02   4:57 PM

using Domain.Passport.Models;
using System.Threading.Tasks;

namespace Domian.Passport.Services.Interfaces
{
    public interface IVerificationService
    {
        Task<VerificationSendResult> Send(string cellphone, Verification.VerificationType type);

        Task<Verification> Use(string guid, Verification.VerificationType type);

        Task<Verification> UseForResetPassport(string guid);

        Task<Verification> UseForSignUp(string guid);

        Task<VerificationVerifyResult> Verify(string cellphone, string code, Verification.VerificationType type);
    }

    public class VerificationSendResult
    {
        public int RemainCount { get; set; }

        public bool Successful { get; set; }
    }

    public class VerificationVerifyResult
    {
        public int RemainCount { get; set; }

        public bool Successful { get; set; }

        public string Token { get; set; }
    }
}