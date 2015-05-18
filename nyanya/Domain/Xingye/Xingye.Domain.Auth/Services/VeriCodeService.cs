// FileInformation: nyanya/Xingye.Domain.Auth/VeriCodeService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   3:26 PM

using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xingye.Domain.Auth.Database;
using Xingye.Domain.Auth.Models;
using Xingye.Domain.Auth.Services.DTO;
using Xingye.Domain.Auth.Services.Interfaces;
using Domian.Database;
using Infrastructure.Lib;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;
using Infrastructure.SMS;

namespace Xingye.Domain.Auth.Services
{
    public class VeriCodeService : IVeriCodeService
    {
        private const int VeriCodeValidity = 30;
        private readonly DbContextFactory dbContextFactory;
        private readonly ISmsService smsService;

        public VeriCodeService(ISmsService smsService, DbContextFactory dbContextFactory)
        {
            this.smsService = smsService;
            this.dbContextFactory = dbContextFactory;
        }

        #region IVeriCodeService Members

        public async Task<SendVeriCodeResult> SendAsync(string cellphone, VeriCode.VeriCodeType type)
        {
            string veriCode;
            VeriCode code;

            using (AuthContext context = this.dbContextFactory.Create<AuthContext>())
            {
                // 时间大于今天开始日期，就一定是今天发送的验证码
                code = await context.Query<VeriCode>().OrderByDescending(c => c.BuildAt)
                    .FirstOrDefaultAsync(c => c.Cellphone == cellphone && c.Type == type && c.BuildAt >= DateTime.Today);

                // 超过5次，停止发送
                if (code != null && code.Times >= 5)
                {
                    return new SendVeriCodeResult { RemainCount = -1, Successful = false };
                }

                veriCode = this.GenerateCode();

                // 少于5次，再次发送
                if (code != null && code.Times < 5)
                {
                    // 增加失败次数
                    code.Times += 1;
                    // 重新生成验证码
                    code.Code += "|" + veriCode;
                    // 重置验证码验证数据
                    code.ErrorCount = 0;
                    code.Verified = false;
                    code.Used = false;
                    code.BuildAt = DateTime.Now;
                }

                // 没有记录，重新生成
                if (code == null)
                {
                    code = new VeriCode
                    {
                        Cellphone = cellphone,
                        Identifier = GuidUtils.NewGuidString(),
                        Code = veriCode,
                        ErrorCount = 0,
                        BuildAt = DateTime.Now,
                        Times = 1,
                        Type = type,
                        Used = false,
                        Verified = false
                    };

                    context.Add(code);
                }

                await context.SaveChangesAsync();
            }

            string verifyMessage = NyanyaResources.Sms_xy_VeriCode.FormatWith(veriCode, VeriCodeValidity);
            // ReSharper disable once UnusedVariable
            Task<bool> smsResult = this.smsService.SendAsync(cellphone, verifyMessage, 1);

            return new SendVeriCodeResult { RemainCount = 5 - code.Times, Successful = true };
        }

        public Task<UseVeriCodeResult> UseAsync(string code, VeriCode.VeriCodeType type)
        {
            VeriCode veriCode = new VeriCode(code);
            return veriCode.UseAsync(type);
        }

        public async Task<VerifyVeriCodeResult> VerifyAsync(string cellphone, string code, VeriCode.VeriCodeType type)
        {
            using (AuthContext context = this.dbContextFactory.Create<AuthContext>())
            {
                // 只取有效期内的验证码
                DateTime availableTime = DateTime.Now.AddMinutes(-VeriCodeValidity);
                VeriCode veriCode = await context.Query<VeriCode>().OrderByDescending(v => v.BuildAt)
                    .FirstOrDefaultAsync(v => v.Cellphone == cellphone && v.Type == type && v.BuildAt >= availableTime);

                // 无该手机验证码记录，或者超过3次，验证码失效
                if (veriCode == null || veriCode.ErrorCount >= 3)
                {
                    return new VerifyVeriCodeResult { RemainCount = -1, Successful = false };
                }

                // 少于3次，执行验证
                if (veriCode.Code.Split(new[] { '|' }).Contains(code))
                {
                    if (!veriCode.Verified)
                    {
                        veriCode.Verified = true;
                        await context.SaveChangesAsync();
                    }
                    // 验证成功，返回token
                    return new VerifyVeriCodeResult { Successful = true, Token = veriCode.Identifier };
                }

                // 验证未通过，且失败次数少于2次，递增失败次数
                veriCode.ErrorCount += 1;
                veriCode.Verified = false;
                await context.SaveChangesAsync();
                return new VerifyVeriCodeResult { Successful = false, RemainCount = 3 - veriCode.ErrorCount };
            }
        }

        #endregion IVeriCodeService Members

        private string GenerateCode()
        {
            Random r = new Random();
            return r.Next(100000, 999999).ToString();
        }
    }
}