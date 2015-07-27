// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : TranscationState.cs
// Created          : 2015-07-27  9:16 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  3:40 PM
// ***********************************************************************
// <copyright file="TranscationState.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

namespace DataTransfer.Models
{
    internal enum TranscationState
    {
        None = 0,
        ChongZhi = 10,
        GouMai = 20,
        BenJin = 30,
        LiXi = 40,
        QuXian = 50,
        ToJBY = 60,
        ToQianBao = 70,
        RecieveByQianBao = 80,
        RecieveByJBY = 90
    }
}