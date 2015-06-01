// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:05 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-01  5:01 PM
// ***********************************************************************
// <copyright file="YilianRequestParameterHelper.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Yuyi.Jinyinmao.Service
{
    internal static class YilianRequestParameterHelper
    {
        internal static string TransformCredentialType(int idType)
        {
            switch (idType)
            {
                case 10:
                    return "0";

                case 20:
                    return "2";

                case 30:
                    return "6";

                case 40:
                    return "3";

                default:
                    return "X";
            }
        }
    }
}