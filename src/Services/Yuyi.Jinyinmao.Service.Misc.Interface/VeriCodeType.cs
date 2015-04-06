// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-06  3:56 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-06  3:56 PM
// ***********************************************************************
// <copyright file="VeriCodeType.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Yuyi.Jinyinmao.Service.Interface
{
    /// <summary>
    ///     Enum VeriCodeType
    /// </summary>
    public enum VeriCodeType
    {
        /// <summary>
        ///     注册
        /// </summary>
        SignUp = 10,

        /// <summary>
        ///     重置登录密码
        /// </summary>
        ResetLoginPassword = 20,

        /// <summary>
        ///     重置支付密码
        /// </summary>
        ResetPaymentPassword = 30
    }
}
