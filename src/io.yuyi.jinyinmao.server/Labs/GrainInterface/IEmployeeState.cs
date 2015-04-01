// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-03-31  11:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-03-31  11:34 PM
// ***********************************************************************
// <copyright file="IEmployeeState.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans;

namespace GrainInterface
{
    public interface IEmployeeState : IGrainState
    {
        int Level { get; set; }

        IManager Manager { get; set; }
    }
}
