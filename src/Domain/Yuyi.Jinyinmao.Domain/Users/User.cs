// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  11:27 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-30  1:52 AM
// ***********************************************************************
// <copyright file="User.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moe.Lib;
using Orleans;
using Orleans.Providers;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.EventProcessor;
using Yuyi.Jinyinmao.Domain.Events;
using Yuyi.Jinyinmao.Domain.Sagas;
using Yuyi.Jinyinmao.Helper;

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

        private Dictionary<string, BankCard> BankCards { get; set; }

        #region IUser Members

        /// <summary>
        ///     Adds the bank card.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task AddBankCardAsync(AddBankCard command)
        {
            if (!this.State.Verified)
            {
                return;
            }

            if (this.BankCards.ContainsKey(command.BankCardNo))
            {
                return;
            }

            this.State.BankCards.Add(new BankCard
            {
                BankCardNo = command.BankCardNo,
                BankName = command.BankName,
                Cellphone = this.State.Cellphone,
                CityName = command.CityName,
                IsDefault = false,
                Verified = false,
                VerifiedByYilian = false,
                VerifiedTime = null
            });

            IAddBankCardSaga saga = AddBankCardSagaFactory.GetGrain(Guid.NewGuid());
            await saga.BeginProcessAsync(new AddBankCardSagaInitDto
            {
                Command = command,
                UserInfo = await this.GetUserInfoAsync()
            });

            await this.StoreCommandAsync(command);
            await this.State.WriteStateAsync();
        }

        /// <summary>
        ///     Adds the bank card asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns>Task.</returns>
        public async Task AddBankCardResultedAsync(AddBankCard command, bool result)
        {
            if (result)
            {
                this.State.BankCards.Where(c => c.BankCardNo == command.BankCardNo).ForEach(
                    c =>
                    {
                        c.Verified = true;
                        c.VerifiedByYilian = true;
                        c.VerifiedTime = DateTime.UtcNow.AddHours(8);
                    });
            }
            else
            {
                this.State.BankCards.RemoveAll(c => !c.Verified && c.BankCardNo == command.BankCardNo);
            }

            await this.RaiseAddBankCardResultedEvent(command, result);
            await this.State.WriteStateAsync();
        }

        /// <summary>
        ///     Authenticatings the user information asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task AuthenticateAsync(Authenticate command)
        {
            if (!this.State.Verified)
            {
                return;
            }

            if (this.BankCards.ContainsKey(command.BankCardNo))
            {
                return;
            }

            this.State.RealName = command.RealName;
            this.State.Credential = command.Credential;
            this.State.CredentialNo = command.CredentialNo;
            this.State.BankCards.Add(new BankCard
            {
                BankCardNo = command.BankCardNo,
                BankName = command.BankName,
                Cellphone = this.State.Cellphone,
                CityName = command.CityName,
                IsDefault = false,
                Verified = false,
                VerifiedByYilian = false,
                VerifiedTime = null
            });

            IAuthenticateSaga saga = AuthenticateSagaFactory.GetGrain(Guid.NewGuid());
            await saga.BeginProcessAsync(new AuthenticateSagaInitDto
            {
                Command = command,
                UserInfo = await this.GetUserInfoAsync()
            });

            await this.StoreCommandAsync(command);
            await this.State.WriteStateAsync();
        }

        /// <summary>
        ///     Authenticatings the user information asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns>Task.</returns>
        public async Task AuthenticateAsync(Authenticate command, bool result)
        {
            DateTime now = DateTime.UtcNow.AddHours(8);
            if (this.State.Verified)
            {
                return;
            }

            if (result)
            {
                this.State.Verified = true;
                this.State.VerifiedTime = now;
                this.State.BankCards.Where(c => c.BankCardNo == command.BankCardNo).ForEach(
                    c =>
                    {
                        c.IsDefault = true;
                        c.Verified = true;
                        c.VerifiedByYilian = true;
                        c.VerifiedTime = now;
                    });
            }
            else
            {
                this.State.RealName = string.Empty;
                this.State.Credential = Credential.None;
                this.State.CredentialNo = string.Empty;
                this.State.Verified = false;
                this.State.VerifiedTime = null;
                this.State.BankCards.RemoveAll(c => !c.Verified && c.BankCardNo == command.BankCardNo);
            }

            await this.RaiseApplyForAuthenticationResultedEvent(command, result);
            await this.State.WriteStateAsync();
        }

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
        ///     Gets the bank card information asynchronous.
        /// </summary>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns>Task&lt;BankCardInfo&gt;.</returns>
        public Task<BankCardInfo> GetBankCardInfoAsync(string bankCardNo)
        {
            BankCard card;
            if (this.BankCards.TryGetValue(bankCardNo, out card))
            {
                return Task.FromResult(new BankCardInfo
                {
                    BankCardNo = card.BankCardNo,
                    BankName = card.BankName,
                    CanBeUsedForYilian = card.VerifiedByYilian,
                    IsDefault = card.IsDefault,
                    VerifiedTime = card.VerifiedTime.GetValueOrDefault(DateTime.MaxValue),
                    WithdrawAmount = 0
                });
            }
            return null;
        }

        /// <summary>
        ///     Gets the bank card infos asynchronous.
        /// </summary>
        /// <returns>Task&lt;List&lt;BankCardInfo&gt;&gt;.</returns>
        public Task<List<BankCardInfo>> GetBankCardInfosAsync()
        {
            return Task.FromResult(this.BankCards.Values.Select(c => new BankCardInfo
            {
                BankCardNo = c.BankCardNo,
                BankName = c.BankName,
                CanBeUsedForYilian = c.VerifiedByYilian,
                IsDefault = c.IsDefault,
                VerifiedTime = c.VerifiedTime.GetValueOrDefault(DateTime.MaxValue),
                WithdrawAmount = c.WithdrawAmount
            }).ToList());
        }

        /// <summary>
        ///     Gets the user information asynchronous.
        /// </summary>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public Task<UserInfo> GetUserInfoAsync()
        {
            BankCard card = this.BankCards.Values.FirstOrDefault(c => c.IsDefault);

            return Task.FromResult(new UserInfo
            {
                BankCardNo = card == null ? string.Empty : card.BankCardNo,
                BankCardsCount = this.BankCards.Count,
                BankName = card == null ? string.Empty : card.BankName,
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

            DateTime registerTime = DateTime.UtcNow.AddHours(8);

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
            await this.RaiseUserRegisteredEvent(command);

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
            await this.RaiseLoginPasswordResetEvent(command);

            await this.State.WriteStateAsync();
        }

        /// <summary>
        ///     Sets the default bank card asynchronous.
        /// </summary>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns>Task.</returns>
        public Task SetDefaultBankCardAsync(string bankCardNo)
        {
            BankCard card;
            if (this.BankCards.TryGetValue(bankCardNo, out card) && !card.IsDefault
                && card.Verified)
            {
                this.State.BankCards.ForEach(c => c.IsDefault = true);
                card.IsDefault = true;
            }

            return this.State.WriteStateAsync();
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
            await this.RaisePaymentPasswordSetEvent(command);

            await this.State.WriteStateAsync();
        }

        #endregion IUser Members

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            this.ReloadBankCardsData();
            return base.OnActivateAsync();
        }

        /// <summary>
        ///     Raises the add bank card resulted event.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns>Task.</returns>
        private async Task RaiseAddBankCardResultedEvent(AddBankCard command, bool result)
        {
            AddBankCardResulted @event = new AddBankCardResulted
            {
                Args = command.Args,
                BankCardNo = command.BankCardNo,
                BankName = command.BankName,
                Cellphone = this.State.Cellphone,
                CityName = command.CityName,
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name,
                CanBeUsedByYilian = true,
                Result = result,
                UserId = this.State.Id,
                Verified = result,
                VerifiedTime = DateTime.UtcNow.AddHours(8)
            };
            await this.StoreEventAsync(@event);

            await AddBankCardResultedProcessorFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);
        }

        private async Task RaiseApplyForAuthenticationResultedEvent(Authenticate command, bool result)
        {
            DateTime now = DateTime.UtcNow.AddHours(8);

            AuthenticateResulted @event = new AuthenticateResulted
            {
                Args = command.Args,
                Cellphone = command.Cellphone,
                Credential = command.Credential,
                CredentialNo = command.CredentialNo,
                RealName = command.RealName,
                Result = result,
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name,
                UserId = this.State.Id,
                Verified = result,
                VerifiedTime = now
            };

            await this.StoreEventAsync(@event);

            await AuthenticateResultedProcessorFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);

            await this.RaiseAddBankCardResultedEvent(new AddBankCard
            {
                Args = command.Args,
                BankCardNo = command.BankCardNo,
                BankName = command.BankName,
                CityName = command.CityName,
                CommandId = command.CommandId,
                UserId = this.State.Id
            }, result);
        }

        /// <summary>
        ///     Raises the login password reset event.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task RaiseLoginPasswordResetEvent(ResetLoginPassword command)
        {
            LoginPasswordReset @event = new LoginPasswordReset
            {
                Args = command.Args,
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name
            };

            await this.StoreEventAsync(@event);

            await LoginPasswordResetProcessorFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);
        }

        private async Task RaisePaymentPasswordSetEvent(SetPaymentPassword command)
        {
            if (command.Override)
            {
                PaymentPasswordReset @event = new PaymentPasswordReset
                {
                    Args = command.Args,
                    SourceId = this.State.Id.ToGuidString(),
                    SourceType = this.GetType().Name
                };

                await this.StoreEventAsync(@event);

                await PaymentPasswordResetFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);
            }
            else
            {
                PaymentPasswordSet @event = new PaymentPasswordSet
                {
                    Args = command.Args,
                    SourceId = this.State.Id.ToGuidString(),
                    SourceType = this.GetType().Name
                };

                await this.StoreEventAsync(new PaymentPasswordSet
                {
                    Args = command.Args,
                    SourceId = this.State.Id.ToGuidString(),
                    SourceType = this.GetType().Name
                });

                await PaymentPasswordSetFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);
            }
        }

        private async Task RaiseUserRegisteredEvent(UserRegister userRegister)
        {
            UserRegistered @event = new UserRegistered
            {
                Args = userRegister.Args,
                Cellphone = this.State.Cellphone,
                ClientType = this.State.ClientType,
                ContractId = this.State.ContractId,
                InviteBy = this.State.InviteBy,
                JBYAccountId = this.State.JBYAccount.GetPrimaryKey(),
                LoginNames = this.State.LoginNames,
                OutletCode = userRegister.OutletCode,
                RegisterTime = this.State.RegisterTime,
                SettlementAccountId = this.State.SettlementAccount.GetPrimaryKey(),
                UserId = userRegister.UserId,
                SourceId = this.State.Id.ToGuidString(),
                SourceType = this.GetType().Name
            };

            await this.StoreEventAsync(@event);

            await UserRegisteredProcessorFactory.GetGrain(@event.EventId).ProcessEventAsync(@event);
        }

        private void ReloadBankCardsData()
        {
            this.BankCards = this.State.BankCards.ToDictionary(c => c.BankCardNo);
        }
    }
}
