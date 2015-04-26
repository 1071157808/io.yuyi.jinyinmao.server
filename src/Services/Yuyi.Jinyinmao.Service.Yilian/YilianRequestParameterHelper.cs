// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  6:15 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-26  6:15 PM
// ***********************************************************************
// <copyright file="YilianRequestParameterHelper.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
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
                case 0:
                    return "0";

                case 1:
                    return "2";

                case 2:
                    return "6";

                case 3:
                    return "3";

                default:
                    return "X";
            }
        }
    }
}
