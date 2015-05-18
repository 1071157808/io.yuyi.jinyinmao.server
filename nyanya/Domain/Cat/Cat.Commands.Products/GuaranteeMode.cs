// ***********************************************************************
// Project          : nyanya
// Author           : Siqi Lu
// Created          : 2015-03-04  6:31 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-28  2:59 PM
// ***********************************************************************
// <copyright file="GuaranteeMode.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Cat.Commands.Products
{
    /// <summary>
    ///     担保方式
    /// </summary>
    public enum GuaranteeMode
    {
        /// <summary>
        ///     银行
        /// </summary>
        Bank = 10,

        /// <summary>
        ///     公司
        /// </summary>
        Company = 20,

        /// <summary>
        ///     国有企业
        /// </summary>
        StateEnterprise = 30,

        /// <summary>
        ///     国有担保公司
        /// </summary>
        StateguaranteeCompany = 40,

        /// <summary>
        ///     担保公司
        /// </summary>
        GuaranteeCompany = 50,

        /// <summary>
        ///     上市集团
        /// </summary>
        ListedGroup = 60,

        /// <summary>
        ///     集团
        /// </summary>
        Group = 70,

        /// <summary>
        ///     部分国有担保公司
        /// </summary>
        PartStateguaranteeCompany = 80,

        /// <summary>
        ///     银行担保
        /// </summary>
        Bankguarantee = 90,

        /// <summary>
        ///     The jinyinmao
        /// </summary>
        Jinyinmao = 100
    }
}
