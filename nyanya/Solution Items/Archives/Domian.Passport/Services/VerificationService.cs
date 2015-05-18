// FileInformation: nyanya/Domian.Passport/VerificationService.cs
// CreatedTime: 2014/05/22   1:56 AM
// LastUpdatedTime: 2014/07/09   9:34 AM

using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.Passport.Models;
using Domian.Passport.Services.Interfaces;
using Infrastructure.SMS;

namespace Domian.Passport.Services
{
    public class VerificationService : IVerificationService
    {
        //private readonly PassportContext db = new PassportContext();
        private readonly ISmsService smsService = new SmsService();

        #region IVerificationService Members

        public async Task<VerificationSendResult> Send(string cellphone, Verification.VerificationType type)
        {
            using (PassportContext db = new PassportContext())
            {
                Verification verification = await db.Verifications.OrderByDescending(v => v.BuildAt).
                    FirstOrDefaultAsync(v => v.Cellphone == cellphone && v.Type == type && v.BuildAt >= DateTime.Today.Date); // 时间大于今天开始日期，就一定是今天发送的验证码

                // 超过5次，停止发送
                if (verification != null && verification.Times >= 5)
                {
                    return new VerificationSendResult { RemainCount = -1, Successful = false };
                }

                string verifyCode = this.GenerateCode();

                // 少于5次，再次发送
                if (verification != null && verification.Times < 5)
                {
                    // 增加失败次数
                    verification.Times += 1;
                    // 重新生成验证码
                    verification.Code += "|" + verifyCode;
                    // 充值验证码验证数据
                    verification.ErrorCount = 0;
                    verification.Verified = false;
                    verification.Used = false;
                    verification.BuildAt = DateTime.Now;
                }

                // 没有记录，重新生成
                if (verification == null)
                {
                    verification = new Verification
                    {
                        Cellphone = cellphone,
                        Guid = Guid.NewGuid().ToString().ToLower().Replace("-", ""),
                        Code = verifyCode,
                        ErrorCount = 0,
                        BuildAt = DateTime.Now,
                        Times = 1,
                        Type = type,
                        Used = false,
                        Verified = false
                    };
                    db.Verifications.Add(verification);
                }

                await db.SaveChangesAsync();

                string verifyMessage = string.Format("您用于金银猫的手机验证码是{0}，为了保护您的账户安全，请妥善保管。", verifyCode);
                // ReSharper disable once UnusedVariable
                Task<bool> smsResult = this.smsService.SendAsync(cellphone, verifyMessage);

                return new VerificationSendResult { RemainCount = 5 - verification.Times, Successful = true };
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public async Task<Verification> Use(string guid, Verification.VerificationType type)
        {
            using (PassportContext db = new PassportContext())
            {
                DateTime availableTime = DateTime.Now.AddMinutes(-10);
                Verification verification = await db.Verifications.OrderByDescending(v => v.BuildAt)
                    .FirstOrDefaultAsync(v => v.Guid == guid && v.Type == type && v.BuildAt >= availableTime);

                if (verification == null || !verification.Verified || verification.Used) return null;

                verification.Used = true;
                await db.SaveChangesAsync();

                return verification;
            }
        }

        public async Task<Verification> UseForResetPassport(string guid)
        {
            return await this.Use(guid, Verification.VerificationType.ResetPassword);
        }

        public async Task<Verification> UseForSignUp(string guid)
        {
            return await this.Use(guid, Verification.VerificationType.SignUp);
        }

        public async Task<VerificationVerifyResult> Verify(string cellphone, string code, Verification.VerificationType type)
        {
            using (PassportContext db = new PassportContext())
            {
                // 只取有效期内的验证码
                DateTime availableTime = DateTime.Now.AddMinutes(-10);
                Verification verification = await db.Verifications.OrderByDescending(v => v.BuildAt)
                    .FirstOrDefaultAsync(v => v.Cellphone == cellphone && v.Type == type && v.BuildAt >= availableTime);

                // 无该手机验证码记录，或者验证码失效
                if (verification == null)
                {
                    return new VerificationVerifyResult { RemainCount = -1, Successful = false };
                }

                // 超过3次，删除该验证码记录，使验证码失效
                if (verification.ErrorCount >= 3)
                {
                    db.Verifications.Remove(verification);
                    await db.SaveChangesAsync();
                    return new VerificationVerifyResult { RemainCount = -1, Successful = false };
                }

                // 少于3次，执行验证
                if (verification.Code.Split(new[] { '|' }).Contains(code))
                {
                    verification.Verified = true;
                    await db.SaveChangesAsync();
                    // 验证成功，返回token
                    return new VerificationVerifyResult { Successful = true, Token = verification.Guid };
                }

                // 验证未通过，已经失败2次，直接删除
                if (verification.ErrorCount == 2)
                {
                    db.Verifications.Remove(verification);
                    await db.SaveChangesAsync();
                    return new VerificationVerifyResult { Successful = false, RemainCount = -1 };
                }

                // 验证未通过，且失败次数少于2次，递增失败次数
                verification.ErrorCount += 1;
                await db.SaveChangesAsync();
                return new VerificationVerifyResult { Successful = false, RemainCount = 3 - verification.ErrorCount };
            }
        }

        #endregion IVerificationService Members

        private string GenerateCode()
        {
            Random r = new Random();
            return r.Next(100000, 999999).ToString();
        }
    }
}