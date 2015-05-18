// FileInformation: nyanya/Cat.Domain.Auth/VeriCode.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/28   4:34 PM

using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cat.Domain.Auth.Database;
using Cat.Domain.Auth.Services.DTO;
using Infrastructure.Lib.Utility;

namespace Cat.Domain.Auth.Models
{
    public partial class VeriCode
    {
        private const int VeriCodeValidity = 30;

        public async Task<UseVeriCodeResult> UseAsync(VeriCodeType type)
        {
            Guard.ArgumentNotNull(type, "VeriCodeType");
            Guard.IdentifierMustBeAssigned(this.Identifier, this.GetType().ToString());

            using (AuthContext context = new AuthContext())
            {
                // 验证码的使用有效期为30分钟
                DateTime availableTime = DateTime.Now.AddMinutes(-VeriCodeValidity);
                VeriCode veriCode = await context.Query<VeriCode>().OrderByDescending(v => v.BuildAt)
                    .FirstOrDefaultAsync(v => v.Identifier == this.Identifier && v.Type == type && v.BuildAt >= availableTime);

                if (veriCode == null || !veriCode.Verified || veriCode.Used) return UseVeriCodeResult.CreateFailedResult();

                veriCode.Used = true;
                await context.SaveChangesAsync();

                return new UseVeriCodeResult
                {
                    Cellphone = veriCode.Cellphone,
                    Result = true
                };
            }
        }
    }
}