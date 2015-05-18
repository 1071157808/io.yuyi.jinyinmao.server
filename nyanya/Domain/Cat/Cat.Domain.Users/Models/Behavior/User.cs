// ***********************************************************************
// Assembly         : nyanya
// Author           : Siqi Lu
// Created          : 2015-03-05  9:54 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-03-26  11:20 AM
// ***********************************************************************
// <copyright file="User.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Cat.Commands.Users;
using Cat.Domain.Users.Database;
using Cat.Domain.Users.Helper;
using Cat.Events.Users;
using Cat.Events.Yilian;
using Domian.Bus;
using Domian.Config;
using Infrastructure.Lib.Exceptions;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;

namespace Cat.Domain.Users.Models
{
    public partial class User
    {
        public User()
        {
            this.UserIdentifier = GuidUtils.NewGuidString();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="User" /> class.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        public User(string userIdentifier)
        {
            this.UserIdentifier = userIdentifier;
        }

        private IEventBus eventbus
        {
            get { return CqrsDomainConfig.Configuration.EventBus; }
        }

        /// <summary>
        ///     Adds the bank card asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public async Task AddBankCardAsync(AddBankCard command)
        {
            BankCardRecord record = new BankCardRecord
            {
                BankCardNo = command.BankCardNo,
                BankName = command.BankName,
                Cellphone = command.Cellphone,
                CityName = command.CityName,
                SequenceNo = SequenceNoUtils.GenerateNo('B'),
                Verified = null,
                VerifiedTime = null,
                UserIdentifier = command.UserIdentifier,
                VerifingTime = DateTime.Now,
                ClientType = command.ClientType,
                FlgMoreI1 = command.FlgMoreI1,
                FlgMoreI2 = command.FlgMoreI2,
                FlgMoreS1 = command.FlgMoreS1,
                FlgMoreS2 = command.FlgMoreS2,
                IpClient = command.IpClient
            };

            using (UserContext context = new UserContext())
            {
                await context.SaveAsync(record);
            }

            this.eventbus.Publish(new AppliedForAddBankCard(this.UserIdentifier, this.GetType())
            {
                BankCardNo = record.BankCardNo,
                BankName = record.BankName,
                Cellphone = command.Cellphone,
                CityName = record.CityName,
                CredentialCode = command.Credential.CredentialTypeCode(),
                CredentialNo = command.CredentialNo,
                RealName = command.RealName,
                SequenceNo = record.SequenceNo
            });
        }

        /// <summary>
        ///     Changes the password asynchronous.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        public async Task ChangePasswordAsync(string password, string salt)
        {
            using (UserContext context = new UserContext())
            {
                UserLoginInfo info = await context.Query<UserLoginInfo>().FirstOrDefaultAsync(u => u.UserIdentifier == this.UserIdentifier);
                info.Salt = salt;
                info.EncryptedPassword = CryptographyUtils.Encrypting(password, salt);
                info.LoginFailedCount = 0;
                await context.SaveChangesAsync();
            }

            this.eventbus.Publish(new PasswordChanged(this.UserIdentifier, this.GetType()));
        }

        /// <summary>
        ///     Sets the payment password asynchronous.
        /// </summary>
        /// <param name="paymentPassword">The payment password.</param>
        /// <returns></returns>
        public async Task SetPaymentPasswordAsync(string paymentPassword)
        {
            using (UserContext context = new UserContext())
            {
                UserPaymentInfo info = await context.Query<UserPaymentInfo>().FirstOrDefaultAsync(u => u.UserIdentifier == this.UserIdentifier);
                if (info.IsNull())
                {
                    info = new UserPaymentInfo
                    {
                        EncryptedPassword = CryptographyUtils.Encrypting(paymentPassword, this.UserIdentifier),
                        FailedCount = 0,
                        LastFailedTime = DateTime.MinValue,
                        Salt = this.UserIdentifier,
                        SetTime = DateTime.Now,
                        UserIdentifier = this.UserIdentifier
                    };

                    context.Add(info);
                }
                else
                {
                    info.EncryptedPassword = CryptographyUtils.Encrypting(paymentPassword, this.UserIdentifier);
                    info.FailedCount = 0;
                }

                await context.SaveChangesAsync();
            }

            this.eventbus.Publish(new PaymentPasswordSet(this.UserIdentifier, this.GetType()));
        }

        /// <summary>
        ///     Signs up asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public async Task SignUpAsync(RegisterANewUser command)
        {
            string salt = this.UserIdentifier;
            string encryptedPassword = CryptographyUtils.Encrypting(command.Password, salt);
            using (UserContext context = new UserContext())
            {
                this.Cellphone = command.Cellphone;
                this.IdentificationCode = command.IdentificionCode;
                this.UserLoginInfo = this.BuidUserLoginInfo(encryptedPassword, command.Cellphone, salt);
                this.UserType = command.UserType;
                this.ClientType = command.ClientType;
                this.InviteBy = command.InviteBy;
                this.ContractId = command.ContractId;
                this.IpReg = command.IpReg;
                this.FlgMoreS1 = command.FlgMoreS1;
                this.FlgMoreS2 = command.FlgMoreS2;
                this.FlgMoreI1 = command.FlgMoreI1;
                this.FlgMoreI2 = command.FlgMoreI2;
                await context.SaveAsync(this);
            }

            this.eventbus.Publish(new RegisteredANewUser(this.UserIdentifier, this.GetType())
            {
                Cellphone = this.Cellphone
            });
        }

        internal async Task GotYilianVerifyResultAsync(IYilianVerifyResultEvent @event)
        {
            if (@event.Result)
            {
                await this.YilianVerifySucceededAsync(@event);
            }
            else
            {
                await this.YilianVerifyFailedAsync(@event);
            }
        }

        internal async Task SignUpPaymentAsync(SignUpPayment command)
        {
            Guard.IdentifierMustBeAssigned(this.UserIdentifier, this.GetType().ToString());

            BankCardRecord record = new BankCardRecord
            {
                BankCardNo = command.BankCardNo,
                BankName = command.BankName,
                Cellphone = command.Cellphone,
                CityName = command.CityName,
                SequenceNo = SequenceNoUtils.GenerateNo('A'),
                Verified = null,
                VerifiedTime = null,
                UserIdentifier = command.UserIdentifier,
                VerifingTime = DateTime.Now,
                ClientType = command.ClientType,
                FlgMoreI1 = command.FlgMoreI1,
                FlgMoreI2 = command.FlgMoreI2,
                FlgMoreS1 = command.FlgMoreS1,
                FlgMoreS2 = command.FlgMoreS2,
                IpClient = command.IpClient
            };

            YLUserInfo info = new YLUserInfo
            {
                Credential = command.Credential,
                CredentialNo = command.CredentialNo,
                RealName = command.RealName,
                UserIdentifier = this.UserIdentifier,
                Verified = false,
                VerifiedTime = null,
                VerifingTime = DateTime.Now
            };

            using (UserContext context = new UserContext())
            {
                context.Add(record);
                context.Add(info);
                await context.SaveChangesAsync();
            }

            this.eventbus.Publish(new AppliedForSignUpPayment(this.UserIdentifier, this.GetType())
            {
                BankCardNo = record.BankCardNo,
                BankName = record.BankName,
                Cellphone = command.Cellphone,
                CityName = record.CityName,
                CredentialCode = info.Credential.CredentialTypeCode(),
                CredentialNo = info.CredentialNo,
                RealName = info.RealName,
                SequenceNo = record.SequenceNo
            });
        }

        /// <summary>
        ///     Buids the user login information.
        /// </summary>
        /// <param name="encryptedPassword">The encrypted password.</param>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        private UserLoginInfo BuidUserLoginInfo(string encryptedPassword, string cellphone, string salt)
        {
            Guard.IdentifierMustBeAssigned(this.UserIdentifier, this.GetType().ToString());

            return new UserLoginInfo
            {
                EncryptedPassword = encryptedPassword,
                LoginName = cellphone,
                Salt = salt,
                SignUpTime = DateTime.Now,
                LastFailedSignInTime = DateTime.MinValue,
                LoginFailedCount = 0,
                UserIdentifier = this.UserIdentifier,
                LastSuccessSignInTime = DateTime.MinValue
            };
        }

        private void PublishYilianVerifyFailedEvent(BankCardRecord record)
        {
            if (record.IsForAuth)
            {
                this.eventbus.Publish(new SignUpPaymentFailed(this.UserIdentifier, this.GetType())
                {
                    BankCardNo = record.BankCardNo,
                    BankName = record.BankName,
                    Cellphone = record.Cellphone,
                    Message = record.Remark
                });
            }
            else
            {
                this.eventbus.Publish(new AddBankCardFailed(this.UserIdentifier, this.GetType())
                {
                    BankCardNo = record.BankCardNo,
                    BankName = record.BankName,
                    Cellphone = record.Cellphone,
                    Message = record.Remark
                });
            }
        }

        private void PublishYilianVerifySucceededEvent(BankCardRecord record)
        {
            if (record.IsForAuth)
            {
                this.eventbus.Publish(new SignUpPaymentSucceeded(this.UserIdentifier, this.GetType())
                {
                    BankCardNo = record.BankCardNo,
                    BankName = record.BankName,
                    Cellphone = record.Cellphone
                });
            }
            else
            {
                this.eventbus.Publish(new AddBankCardSucceeded(this.UserIdentifier, this.GetType())
                {
                    BankCardNo = record.BankCardNo,
                    BankName = record.BankName,
                    Cellphone = record.Cellphone
                });
            }
        }

        private async Task YilianVerifyFailedAsync(IYilianVerifyResultEvent @event)
        {
            Guard.IdentifierMustBeAssigned(this.UserIdentifier, this.GetType().ToString());
            await this.YilianVerifyFailedAsync(@event.SequenceNo, @event.Message);
        }

        private async Task YilianVerifyFailedAsync(string sequenceNo, string message)
        {
            BankCardRecord record;
            using (UserContext context = new UserContext())
            {
                record = await context.Query<BankCardRecord>().FirstAsync(r => r.SequenceNo == sequenceNo && r.UserIdentifier == this.UserIdentifier);

                // 以第一次结果为准，忽略后续结果
                if (record.Verified.HasValue)
                {
                    return;
                }

                if (record.IsForAuth)
                {
                    User user = await context.Query<User>().Include(u => u.YLUserInfo).FirstOrDefaultAsync(u => u.UserIdentifier == this.UserIdentifier);

                    if (user == null)
                    {
                        throw new ApplicationBusinessException("Data missing.User {0}".FmtWith(this.UserIdentifier));
                    }

                    if (user.YLUserInfo == null)
                    {
                        throw new ApplicationBusinessException("User YLUserInfo has been removed before Yilian verified {0} for user {1}".FmtWith(this.UserIdentifier, sequenceNo));
                    }

                    user.YLUserInfo = null;
                }

                record.Verified = false;
                record.VerifiedTime = DateTime.Now;
                record.Remark = message;

                await context.SaveChangesAsync();
            }

            this.PublishYilianVerifyFailedEvent(record);
        }

        private async Task YilianVerifySucceededAsync(IYilianVerifyResultEvent @event)
        {
            Guard.IdentifierMustBeAssigned(this.UserIdentifier, this.GetType().ToString());
            await this.YilianVerifySucceededAsync(@event.SequenceNo, @event.Message);
        }

        private async Task YilianVerifySucceededAsync(string sequenceNo, string message)
        {
            BankCardRecord record;
            using (UserContext context = new UserContext())
            {
                User user;
                record = await context.Query<BankCardRecord>().FirstAsync(r => r.SequenceNo == sequenceNo && r.UserIdentifier == this.UserIdentifier);

                // 以第一次结果为准，忽略后续结果
                if (record.Verified.HasValue)
                {
                    return;
                }

                user = await context.Query<User>().Include(u => u.YLUserInfo).Include(u => u.BankCards).FirstOrDefaultAsync(u => u.UserIdentifier == this.UserIdentifier);

                if (user == null || user.BankCards == null)
                {
                    throw new ApplicationBusinessException("Data missing.User {0}".FmtWith(this.UserIdentifier));
                }

                if (user.YLUserInfo == null)
                {
                    throw new ApplicationBusinessException("User YLUserInfo has been removed before Yilian verified {0} for user {1}".FmtWith(this.UserIdentifier, sequenceNo));
                }

                if (record.IsForAuth)
                {
                    user.YLUserInfo.Verified = true;
                    user.YLUserInfo.VerifiedTime = DateTime.Now;
                }

                record.Verified = true;
                record.VerifiedTime = DateTime.Now;
                record.Remark = message;

                user.BankCards.Add(new BankCard
                {
                    BankCardNo = record.BankCardNo,
                    BankName = record.BankName,
                    CityName = record.CityName,
                    IsDefault = record.IsForAuth,
                    UserIdentifier = this.UserIdentifier,
                    VerifiedTime = DateTime.Now
                });
                await context.SaveChangesAsync();
            }

            this.PublishYilianVerifySucceededEvent(record);
        }
    }
}
