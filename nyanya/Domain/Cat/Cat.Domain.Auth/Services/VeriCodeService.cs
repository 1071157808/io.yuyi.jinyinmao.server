// ***********************************************************************
// Project          : nyanya
// Author           : Siqi Lu
// Created          : 2015-03-04  6:31 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-09  1:25 PM
// ***********************************************************************
// <copyright file="VeriCodeService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Cat.Domain.Auth.Database;
using Cat.Domain.Auth.Models;
using Cat.Domain.Auth.Services.DTO;
using Cat.Domain.Auth.Services.Interfaces;
using Domian.Database;
using Infrastructure.Lib;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;
using Infrastructure.Lib.VerifyCode;
using Infrastructure.SMS;

namespace Cat.Domain.Auth.Services
{
    public class VeriCodeService : IVeriCodeService
    {
        private const int VeriCodeValidity = 30;
        private readonly DbContextFactory dbContextFactory;
        private readonly IParameterService parameterService;
        private readonly ISmsService smsService;

        public VeriCodeService(ISmsService smsService, DbContextFactory dbContextFactory, IParameterService parameterService)
        {
            this.smsService = smsService;
            this.dbContextFactory = dbContextFactory;
            this.parameterService = parameterService;
        }

        #region IVeriCodeService Members

        public async Task<SendVeriCodeResult> SendAsync(string cellphone, VeriCode.VeriCodeType type)
        {
            string veriCode;
            VeriCode code;
            int maxSendTimes = await GetMaxSendTimes(type);

            using (AuthContext context = this.dbContextFactory.Create<AuthContext>())
            {
                // 时间大于今天开始日期，就一定是今天发送的验证码
                code = await context.Query<VeriCode>().OrderByDescending(c => c.BuildAt)
                    .FirstOrDefaultAsync(c => c.Cellphone == cellphone && c.Type == type && c.BuildAt >= DateTime.Today);

                // 超过最大次数，停止发送
                if (code != null && code.Times >= maxSendTimes)
                {
                    return new SendVeriCodeResult { RemainCount = -1, Successful = false };
                }

                veriCode = this.GenerateCode();

                // 小于最大次数，再次发送
                if (code != null && code.Times < maxSendTimes)
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

            string verifyMessage = NyanyaResources.Sms_VeriCode.FormatWith(veriCode, VeriCodeValidity);
            // ReSharper disable once UnusedVariable
            var smsResult = await this.smsService.SendAsync(cellphone, verifyMessage);

            return new SendVeriCodeResult { RemainCount = maxSendTimes - code.Times, Successful = true };
        }

        public async Task<SendVeriCodePhoneResult> SendGraphicAsync(string identifier, int imageWidth, string clientId = "")
        {
            string veriCode;
            VeriCode code;
            //byte[] bitmap;
            Image bitmap;
            clientId = ValidateStringLength(clientId);

            using (AuthContext context = new AuthContext())
            {
                // 时间大于今天开始日期，就一定是今天发送的验证码
                if (clientId.IsNullOrEmpty())
                {
                    code = await context.Query<VeriCode>().OrderByDescending(v => v.BuildAt)
                        .FirstOrDefaultAsync(c => c.Identifier == identifier && c.Type == VeriCode.VeriCodeType.VeriImage && c.BuildAt >= DateTime.Today);
                }
                else
                {
                    code = await context.Query<VeriCode>().OrderByDescending(v => v.BuildAt)
                        .FirstOrDefaultAsync(c => c.ClientId == clientId && c.Type == VeriCode.VeriCodeType.VeriImage && c.BuildAt >= DateTime.Today);
                }

                bitmap = this.GenerateImage(out veriCode, imageWidth);

                // 更新验证图片
                if (code != null)
                {
                    // 增加失败次数
                    code.Times += 1;
                    // 重新生成验证码
                    code.Code = veriCode;
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
                        Cellphone = "13000000000",
                        Identifier = GuidUtils.NewGuidString(),
                        Code = veriCode,
                        ErrorCount = 0,
                        BuildAt = DateTime.Now,
                        Times = 1,
                        Type = VeriCode.VeriCodeType.VeriImage,
                        Used = false,
                        Verified = false,
                        ClientId = clientId //  请求来自app，用设备号作为Token
                    };

                    context.Add(code);
                }
                await context.SaveChangesAsync();
            }
            return new SendVeriCodePhoneResult { Identifier = code.Identifier, VerifyImage = bitmap };
        }

        public async Task<SendVeriCodeResult> SendWithTokenAsync(string cellphone, string token, VeriCode.VeriCodeType type)
        {
            string veriCode;
            VeriCode code;
            int maxSendTimes = await GetMaxSendTimes(type);

            using (AuthContext context = this.dbContextFactory.Create<AuthContext>())
            {
                // 时间大于今天开始日期，就一定是今天发送的验证码
                VeriCode imageCode = await context.Query<VeriCode>().OrderByDescending(c => c.BuildAt)
                    .FirstOrDefaultAsync(c => c.Identifier == token && c.Cellphone == cellphone && c.BuildAt >= DateTime.Today);

                // 当前请求TOKEN必须和图形验证码TOKEN一致
                if (imageCode == null)
                {
                    return new SendVeriCodeResult { RemainCount = -1, Successful = false };
                }

                code = await context.Query<VeriCode>().OrderByDescending(c => c.BuildAt)
                    .FirstOrDefaultAsync(c => c.Type == type && c.Cellphone == cellphone && c.BuildAt >= DateTime.Today);

                // 超过5次，停止发送
                if (code != null && code.Times >= maxSendTimes)
                {
                    return new SendVeriCodeResult { RemainCount = -1, Successful = false };
                }

                veriCode = this.GenerateCode();

                // 少于5次，再次发送
                if (code != null && code.Times < maxSendTimes)
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

            string verifyMessage = NyanyaResources.Sms_VeriCode.FormatWith(veriCode, VeriCodeValidity);
            // ReSharper disable once UnusedVariable
            var smsResult = this.smsService.SendAsync(cellphone, verifyMessage);

            return new SendVeriCodeResult { RemainCount = maxSendTimes - code.Times, Successful = true };
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
                if (veriCode.Code.Split('|').Contains(code))
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

        public async Task<VerifyVeriCodeResult> VerifyGraphicAsync(string cellphone, string identifier, string addResult, string clientId = "")
        {
            clientId = ValidateStringLength(clientId);

            using (AuthContext context = new AuthContext())
            {
                // 只取有效期内的验证码
                DateTime availableTime = DateTime.Now.AddMinutes(-VeriCodeValidity);
                VeriCode veriCode = null;
                if (clientId.IsNullOrEmpty())
                {
                    veriCode = await context.Query<VeriCode>().OrderByDescending(v => v.BuildAt)
                        .FirstOrDefaultAsync(v => v.Identifier == identifier && v.Type == VeriCode.VeriCodeType.VeriImage && v.BuildAt >= availableTime);
                }
                else
                {
                    veriCode = await context.Query<VeriCode>().OrderByDescending(v => v.BuildAt)
                        .FirstOrDefaultAsync(v => v.ClientId == clientId && v.Type == VeriCode.VeriCodeType.VeriImage && v.BuildAt >= availableTime);
                }

                if (veriCode == null || veriCode.ErrorCount >= 3)
                {
                    return new VerifyVeriCodeResult { Successful = false, RemainCount = 3 - veriCode.ErrorCount };
                }
                if (veriCode.Code != addResult)
                {
                    veriCode.ErrorCount++;
                    await context.SaveChangesAsync();
                    return new VerifyVeriCodeResult { Successful = false, RemainCount = 3 - veriCode.ErrorCount };
                }

                veriCode.Verified = true;
                veriCode.Cellphone = cellphone;
                context.Add(veriCode);
                await context.SaveChangesAsync();
                return new VerifyVeriCodeResult { Successful = true, Token = veriCode.Identifier, RemainCount = 3 - veriCode.ErrorCount };
            }
        }

        #endregion IVeriCodeService Members

        private string GenerateCode()
        {
            Random r = new Random();
            return r.Next(100000, 999999).ToString();
        }

        private Image GenerateImage(out string addResult, int imageWidth)
        {
            string code;
            var vCode = new ValidateCoder
            {
                RandomColor = true,
                RandomItalic = true,
                HasBorder = true,
                RandomLineCount = 4,
                RandomPointPercent = 7.0f
            };
            var vCodeImage = vCode.CreateImage(5, out code, imageWidth);
            addResult = code;

            return vCodeImage;
        }

        private async Task<int> GetMaxSendTimes(VeriCode.VeriCodeType type)
        {
            try
            {
                return Convert.ToInt32(await parameterService.GetValue(GetParameterName(type)));
            }
            catch
            {
                return 5;
            }
        }

        private string GetParameterName(VeriCode.VeriCodeType type)
        {
            var parameterName = string.Empty;
            switch (type)
            {
                case VeriCode.VeriCodeType.SignUp:
                    parameterName = "SmsMaxSendTimes_SignUp";
                    break;

                case VeriCode.VeriCodeType.ResetLoginPassword:
                    parameterName = "SmsMaxSendTimes_ResetLoginPassword";
                    break;

                case VeriCode.VeriCodeType.ResetPaymentPassword:
                    parameterName = "SmsMaxSendTimes_ResetPaymentPassword";
                    break;
            }
            return parameterName;
        }

        private string ValidateStringLength(string value)
        {
            if (value.IsNullOrEmpty()) return string.Empty;
            return value.Length > 50 ? value.Substring(0, 50) : value;
        }
    }
}
