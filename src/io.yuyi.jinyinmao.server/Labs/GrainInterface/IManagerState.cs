// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-03-31  11:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-03-31  11:34 PM
// ***********************************************************************
// <copyright file="IManagerState.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using Orleans;

namespace GrainInterface
{
    public interface IManagerState : IGrainState
    {
        List<IEmployee> Reports { get; set; }
    }
}
