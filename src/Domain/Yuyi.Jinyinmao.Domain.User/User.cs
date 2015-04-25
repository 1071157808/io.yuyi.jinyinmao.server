// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-25  11:22 AM
// ***********************************************************************
// <copyright file="User.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moe.Lib;
using Orleans;
using Orleans.Providers;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dto;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.Events;
using Yuyi.Jinyinmao.Domain.Helper;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Class User.
    /// </summary>
    [StorageProvider(ProviderName = "SqlDatabase")]
    public class User : EntityGrain<IUserState>, IUser
    {
        /// <summary>
        ///     Gets or sets the error count.
        /// </summary>
        /// <value>The error count.</value>
        public int ErrorCount { get; set; }

        #region IUser Members

        /// <summary>
        ///     Checks the password asynchronous.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;CheckPasswordResult&gt;.</returns>
        public Task<CheckPasswordResult> CheckPasswordAsync(string loginName, string password)
        {
            if (this.State.Cellphone.IsNullOrEmpty())
            {
                return Task.FromResult(new CheckPasswordResult
                {
                    Success = false,
                    UserExist = false
                });
            }

            if (this.State.Cellphone == loginName || this.State.LoginNames.Contains(loginName))
            {
                if (CryptographyHelper.Check(password, this.State.Salt, this.State.EncryptedPassword))
                {
                    this.ErrorCount = 0;
                    return Task.FromResult(new CheckPasswordResult
                    {
                        Cellphone = this.State.Cellphone,
                        ErrorCount = 0,
                        Success = true,
                        UserExist = true,
                        UserId = this.State.Id
                    });
                }
            }

            this.ErrorCount += 1;

            return Task.FromResult(new CheckPasswordResult
            {
                Cellphone = this.State.Cellphone,
                ErrorCount = this.ErrorCount,
                Success = false,
                UserExist = false,
                UserId = this.State.Id
            });
        }

        /// <summary>
        ///     Checks the password asynchronous.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public Task<bool> CheckPasswordAsync(string password)
        {
            return Task.FromResult(CryptographyHelper.Check(password, this.State.Salt, this.State.EncryptedPassword));
        }

        /// <summary>
        ///     Gets the user information asynchronous.
        /// </summary>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public Task<UserInfo> GetUserInfoAsync()
        {
            return Task.FromResult(new UserInfo
            {
                Cellphone = this.State.Cellphone,
                ContractId = this.State.ContractId,
                Credential = this.State.Credential,
                CredentialNo = this.State.CredentialNo,
                HaSetPaymentPassword = this.State.EncryptedPaymentPassword.IsNotNullOrEmpty(),
                HasSetPassword = this.State.EncryptedPassword.IsNotNullOrEmpty(),
                InviteBy = this.State.InviteBy,
                JBYAccountId = this.State.JBYAccount.GetPrimaryKey(),
                LoginNames = this.State.LoginNames,
                RealName = this.State.RealName,
                RegisterTime = this.State.RegisterTime,
                SettlementAccountId = this.State.SettlementAccount.GetPrimaryKey(),
                UserId = this.State.Id,
                Verified = this.State.Verified,
                VerifiedTime = this.State.VerifiedTime
            });
        }

        /// <summary>
        ///     Determines whether [is registered] asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public Task<bool> IsRegisteredAsync()
        {
            return Task.FromResult(this.State.Cellphone.IsNotNullOrEmpty() && this.State.RegisterTime > DateTime.MinValue);
        }

        /// <summary>
        ///     Registers the specified user register.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Task.</returns>
        public async Task RegisterAsync(UserRegister command)
        {
            if (this.State.Id == command.UserId)
            {
                return;
            }

            if (this.State.Id != Guid.Empty)
            {
                this.GetLogger().Warn(1, "Conflict user id: UserId {0}, UserRegisterCommand.UserId {1}", this.State.Id, command.UserId);
                return;
            }

            DateTime registerTime = DateTime.Now;

            this.State.Id = command.UserId;
            this.State.Args = command.Args;
            this.State.Cellphone = command.Cellphone;
            this.State.ClientType = command.ClientType;
            this.State.ContractId = command.ContractId;
            this.State.Credential = Credential.None;
            this.State.CredentialNo = string.Empty;
            this.State.EncryptedPassword = CryptographyHelper.Encrypting(command.Password, command.UserId.ToGuidString());
            this.State.EncryptedPaymentPassword = string.Empty;
            this.State.InviteBy = command.InviteBy;
            this.State.LoginNames = new List<string> { command.Cellphone };
            this.State.OutletCode = command.OutletCode;
            this.State.PaymentSalt = string.Empty;
            this.State.RealName = string.Empty;
            this.State.RegisterTime = registerTime;
            this.State.Salt = command.UserId.ToGuidString();
            this.State.Verified = false;
            this.State.VerifiedTime = null;

            Guid JBYAccountId = Guid.NewGuid();
            this.State.JBYAccount = JBYAccountFactory.GetGrain(JBYAccountId);
            await this.State.JBYAccount.Register(new JBYAccountRegister
            {
                UserId = command.UserId
            });

            Guid settlementAccountId = Guid.NewGuid();
            this.State.SettlementAccount = SettlementAccountFactory.GetGrain(settlementAccountId);
            await this.State.SettlementAccount.Register(new SettlementAccountRegister
            {
                UserId = command.UserId
            });

            await this.StoreCommandAsync(command);
            await this.RaiseUserRegisteredEvent(command, registerTime, JBYAccountId, settlementAccountId);

            await this.State.WriteStateAsync();
        }

        /// <summary>
        ///     Resets the login password.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task ResetLoginPasswordAsync(ResetLoginPassword command)
        {
            this.State.Salt = command.Salt;
            this.State.EncryptedPassword = CryptographyHelper.Encrypting(command.Password, command.Salt);
            await this.StoreCommandAsync(command);
            await this.RaiseLoginPasswordResetEvent();

            await this.State.WriteStateAsync();
        }

        /// <summary>
        ///     Sets the payment password asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task SetPaymentPasswordAsync(SetPaymentPassword command)
        {
            if (this.State.EncryptedPaymentPassword.IsNullOrEmpty() && !command.Override)
            {
                return;
            }

            this.State.PaymentSalt = command.Salt;
            this.State.EncryptedPaymentPassword = CryptographyHelper.Encrypting(command.PaymentPassword, command.Salt);

            await this.StoreCommandAsync(command);
            await this.RaisePaymentPasswordSetEvent(command.Override);

            await this.State.WriteStateAsync();
        }

        #endregion IUser Members

        /// <summary>
        ///     Raises the login password reset event.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private async Task RaiseLoginPasswordResetEvent()
        {
            await this.StoreEventAsync(new PaymentPasswordReset
            {
                EventId = Guid.NewGuid(),
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name
            });
        }

        private async Task RaisePaymentPasswordSetEvent(bool @override)
        {
            if (@override)
            {
                await this.StoreEventAsync(new PaymentPasswordReset
                {
                    EventId = Guid.NewGuid(),
                    SourceId = this.State.Id.ToGuidString(),
                    SourceType = this.GetType().Name
                });
            }
            else
            {
                await this.StoreEventAsync(new PaymentPasswordSet
                {
                    EventId = Guid.NewGuid(),
                    SourceId = this.State.Id.ToGuidString(),
                    SourceType = this.GetType().Name
                });
            }
        }

        private async Task RaiseUserRegisteredEvent(UserRegister userRegister, DateTime registerTime, Guid JBYAccountId, Guid settlementAccountId)
        {
            UserRegistered @event = new UserRegistered
            {
                Args = userRegister.Args,
                Cellphone = userRegister.Cellphone,
                ClientType = userRegister.ClientType,
                ContractId = userRegister.ContractId,
                InviteBy = userRegister.InviteBy,
                JBYAccountId = JBYAccountId,
                LoginNames = this.State.LoginNames,
                OutletCode = userRegister.OutletCode,
                RegisterTime = registerTime,
                SettlementAccountId = settlementAccountId,
                UserId = userRegister.UserId,
                EventId = Guid.NewGuid(),
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name
            };

            await this.StoreEventAsync(@event);

            await UserRegisteredProcessorFactory.GetGrain(Guid.NewGuid()).ProcessEventAsync(@event);
        }
    }
}
