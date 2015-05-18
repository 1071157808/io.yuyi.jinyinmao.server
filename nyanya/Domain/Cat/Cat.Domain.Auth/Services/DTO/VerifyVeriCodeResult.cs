// FileInformation: nyanya/Cat.Domain.Auth/VerifyVeriCodeResult.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   3:26 PM

namespace Cat.Domain.Auth.Services.DTO
{
    public class VerifyVeriCodeResult
    {
        public int RemainCount { get; set; }

        public bool Successful { get; set; }

        public string Token { get; set; }
    }
}