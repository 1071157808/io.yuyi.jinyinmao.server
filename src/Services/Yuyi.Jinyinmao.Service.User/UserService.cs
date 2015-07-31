// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : UserService.cs
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-31  12:33 PM
// ***********************************************************************
// <copyright file="UserService.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Commands;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Domain.Models;
using Yuyi.Jinyinmao.Domain.Sagas;
using Yuyi.Jinyinmao.Service.Dtos;
using Yuyi.Jinyinmao.Service.Interface;
using BankCard = Yuyi.Jinyinmao.Domain.Models.BankCard;

namespace Yuyi.Jinyinmao.Service
{
    /// <summary>
    ///     Class UserService.
    /// </summary>
    public class UserService : IUserService
    {
        #region IUserService Members

        /// <summary>
        ///     Adds the bank card asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task<BankCardInfo> AddBankCardAsync(AddBankCard command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.AddBankCardAsync(command);
        }

        /// <summary>
        ///     Adds the bank card asynchronous.
        /// </summary>
        /// <param name="addBankCardCommand">The add bank card command.</param>
        /// <param name="verifyBankCardCommand">The verify bank card command.</param>
        /// <returns>Task.</returns>
        public async Task AddBankCardAsync(AddBankCard addBankCardCommand, VerifyBankCard verifyBankCardCommand)
        {
            IUser user = UserFactory.GetGrain(addBankCardCommand.UserId);
            UserInfo userInfo = await user.GetUserInfoAsync();

            IDepositSaga saga = DepositSagaFactory.GetGrain(addBankCardCommand.CommandId);
            await saga.BeginProcessAsync(new DepositSagaInitData
            {
                AddBankCardCommand = addBankCardCommand,
                AuthenticateCommand = null,
                InitUserInfo = userInfo,
                PayByLianlianCommand = null,
                PayByYilianCommand = null,
                VerifyBankCardCommand = verifyBankCardCommand
            });
        }

        /// <summary>
        ///     Adds the extra interest to order.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;OrderInfo&gt;.</returns>
        public Task<OrderInfo> AddExtraInterestToOrderAsync(AddExtraInterest command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.AddExtraInterestToOrderAsync(command);
        }

        /// <summary>
        ///     Authenticatings the asynchronous.
        /// </summary>
        /// <param name="command">The apply for authentication.</param>
        /// <returns>Task.</returns>
        public Task AuthenticateAsync(Authenticate command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.AuthenticateAsync(command);
        }

        /// <summary>
        ///     Authenticates the asynchronous.
        /// </summary>
        /// <param name="addBankCardCommand">The add bank card command.</param>
        /// <param name="authenticateCommand">The authenticate command.</param>
        /// <returns>Task.</returns>
        public async Task AuthenticateAsync(AddBankCard addBankCardCommand, Authenticate authenticateCommand)
        {
            IUser user = UserFactory.GetGrain(addBankCardCommand.UserId);
            UserInfo userInfo = await user.GetUserInfoAsync();

            IDepositSaga saga = DepositSagaFactory.GetGrain(addBankCardCommand.CommandId);
            await saga.BeginProcessAsync(new DepositSagaInitData
            {
                AddBankCardCommand = addBankCardCommand,
                AuthenticateCommand = authenticateCommand,
                InitUserInfo = userInfo,
                PayByLianlianCommand = null,
                PayByYilianCommand = null,
                VerifyBankCardCommand = null
            });
        }

        /// <summary>
        ///     Checks the bank card used asynchronous.
        /// </summary>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> CheckBankCardUsedAsync(string bankCardNo)
        {
            using (JYMDBContext db = new JYMDBContext())
            {
                return await db.ReadonlyQuery<BankCard>().AnyAsync(c => c.BankCardNo == bankCardNo && c.Verified && c.Display);
            }
        }

        /// <summary>
        ///     Checks the cellphone asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>Task&lt;CheckCellphoneResult&gt;.</returns>
        public async Task<CheckCellphoneResult> CheckCellphoneAsync(string cellphone)
        {
            ICellphone cellphoneGrain = CellphoneFactory.GetGrain(GrainTypeHelper.GetCellphoneGrainTypeLongKey(cellphone));
            CellphoneInfo info = await cellphoneGrain.GetCellphoneInfoAsync();

            return new CheckCellphoneResult
            {
                Result = info.Registered
            };
        }

        /// <summary>
        ///     Checks the credential no used asynchronous.
        /// </summary>
        /// <param name="credentialNo">The credential no.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> CheckCredentialNoUsedAsync(string credentialNo)
        {
            using (JYMDBContext db = new JYMDBContext())
            {
                return await db.ReadonlyQuery<User>().AnyAsync(c => c.CredentialNo == credentialNo && c.Verified);
            }
        }

        /// <summary>
        ///     Checks the password asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;SignInResult&gt;.</returns>
        public Task<bool> CheckPasswordAsync(Guid userId, string password)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.CheckPasswordAsync(password);
        }

        /// <summary>
        ///     Sign in asynchronous.
        /// </summary>
        /// <param name="cellphone"></param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;SignInResult&gt;.</returns>
        public async Task<SignInResult> CheckPasswordViaCellphoneAsync(string cellphone, string password)
        {
            ICellphone cellphoneGrain = CellphoneFactory.GetGrain(GrainTypeHelper.GetCellphoneGrainTypeLongKey(cellphone));
            CellphoneInfo info = await cellphoneGrain.GetCellphoneInfoAsync();
            IUser user = UserFactory.GetGrain(info.UserId);
            CheckPasswordResult result = await user.CheckPasswordAsync(cellphone, password);
            return new SignInResult
            {
                Cellphone = result.Cellphone,
                RemainCount = result.RemainCount,
                Success = result.Success,
                UserExist = result.UserExist,
                UserId = result.UserId
            };
        }

        /// <summary>
        ///     Checks the payment password asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="paymentPassword">The payment password.</param>
        /// <returns>Task&lt;CheckPaymentPasswordResult&gt;.</returns>
        public Task<CheckPaymentPasswordResult> CheckPaymentPasswordAsync(Guid userId, string paymentPassword)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.CheckPaymentPasswordAsync(paymentPassword);
        }

        /// <summary>
        ///     Clears the unauthenticated information.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public Task ClearUnauthenticatedInfo(Guid userId)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.ClearUnauthenticatedInfoAsync();
        }

        /// <summary>
        ///     Deposits the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public async Task DepositAsync(PayByYilian command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            UserInfo userInfo = await user.GetUserInfoAsync();

            IDepositSaga saga = DepositSagaFactory.GetGrain(command.CommandId);
            await saga.BeginProcessAsync(new DepositSagaInitData
            {
                AddBankCardCommand = null,
                AuthenticateCommand = null,
                InitUserInfo = userInfo,
                PayByLianlianCommand = null,
                PayByYilianCommand = command,
                VerifyBankCardCommand = null
            });
        }

        /// <summary>
        ///     Gets the bank card information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="bankCardNo">The bank card no.</param>
        /// <returns>Task&lt;BankCardInfo&gt;.</returns>
        public Task<BankCardInfo> GetBankCardInfoAsync(Guid userId, string bankCardNo)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetBankCardInfoAsync(bankCardNo);
        }

        /// <summary>
        ///     Gets the bank card infos asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;List&lt;BankCardInfo&gt;&gt;.</returns>
        public Task<List<BankCardInfo>> GetBankCardInfosAsync(Guid userId)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetBankCardInfosAsync();
        }

        /// <summary>
        ///     Gets the jby account information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;JBYAccountInfo&gt;.</returns>
        public Task<JBYAccountInfo> GetJBYAccountInfoAsync(Guid userId)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetJBYAccountInfoAsync();
        }

        /// <summary>
        ///     Gets the jby account reinvesting transaction infos asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Task&lt;PaginatedList&lt;JBYAccountTransactionInfo&gt;&gt;.</returns>
        public Task<PaginatedList<JBYAccountTransactionInfo>> GetJBYAccountReinvestingTransactionInfosAsync(Guid userId, int pageIndex, int pageSize)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetJBYAccountReinvestingTransactionInfosAsync(pageIndex, pageSize);
        }

        /// <summary>
        ///     Gets the jby account transaction information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>Task&lt;JBYAccountTransactionInfo&gt;.</returns>
        public Task<JBYAccountTransactionInfo> GetJBYAccountTransactionInfoAsync(Guid userId, Guid transactionId)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetJBYAccountTransactionInfoAsync(transactionId);
        }

        /// <summary>
        ///     Gets the jby account transaction infos asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Task&lt;PaginatedList&lt;JBYAccountTransactionInfo&gt;&gt;.</returns>
        public Task<PaginatedList<JBYAccountTransactionInfo>> GetJBYAccountTransactionInfosAsync(Guid userId, int pageIndex, int pageSize)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetJBYAccountTransactionInfosAsync(pageIndex, pageSize);
        }

        /// <summary>
        ///     Gets the order information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="orderId">The order identifier.</param>
        /// <returns>Task&lt;OrderInfo&gt;.</returns>
        public Task<OrderInfo> GetOrderInfoAsync(Guid userId, Guid orderId)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetOrderInfoAsync(orderId);
        }

        /// <summary>
        ///     Gets the order infos asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="ordersSortMode">The orders sort mode.</param>
        /// <returns>Task&lt;PaginatedList&lt;OrderInfo&gt;&gt;.</returns>
        public async Task<PaginatedList<OrderInfo>> GetOrderInfosAsync(Guid userId, int pageIndex, int pageSize, OrdersSortMode ordersSortMode)
        {
            IUser user = UserFactory.GetGrain(userId);
            Tuple<int, List<OrderInfo>> result = await user.GetOrderInfosAsync(pageIndex, pageSize, ordersSortMode);
            return new PaginatedList<OrderInfo>(pageIndex, pageSize, result.Item1, result.Item2);
        }

        /// <summary>
        ///     Gets the order infos asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="ordersSortMode">The orders sort mode.</param>
        /// <param name="categories">The categories.</param>
        /// <returns>PaginatedList&lt;OrderInfo&gt;.</returns>
        public async Task<PaginatedList<OrderInfo>> GetOrderInfosAsync(Guid userId, int pageIndex, int pageSize, OrdersSortMode ordersSortMode, long[] categories)
        {
            IUser user = UserFactory.GetGrain(userId);
            Tuple<int, List<OrderInfo>> result = await user.GetOrderInfosAsync(pageIndex, pageSize, ordersSortMode, categories);
            return new PaginatedList<OrderInfo>(pageIndex, pageSize, result.Item1, result.Item2);
        }

        /// <summary>
        ///     Gets the settle account information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;SettleAccountInfo&gt;.</returns>
        public Task<SettleAccountInfo> GetSettleAccountInfoAsync(Guid userId)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetSettleAccountInfoAsync();
        }

        /// <summary>
        ///     Gets the settle account transaction information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>Task&lt;SettleAccountTransactionInfo&gt;.</returns>
        public Task<SettleAccountTransactionInfo> GetSettleAccountTransactionInfoAsync(Guid userId, Guid transactionId)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetSettleAccountTransactionInfoAsync(transactionId);
        }

        /// <summary>
        ///     Gets the settle account transaction infos asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Task&lt;PaginatedList&lt;SettleAccountTransactionInfo&gt;&gt;.</returns>
        public Task<PaginatedList<SettleAccountTransactionInfo>> GetSettleAccountTransactionInfosAsync(Guid userId, int pageIndex, int pageSize)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetSettleAccountTransactionInfosAsync(pageIndex, pageSize);
        }

        /// <summary>
        ///     Gets the settling order infos asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="count">The count.</param>
        /// <returns>Task&lt;List&lt;OrderInfo&gt;&gt;.</returns>
        public Task<List<OrderInfo>> GetSettlingOrderInfosAsync(Guid userId, int count)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetSettlingOrderInfosAsync(count);
        }

        /// <summary>
        ///     Gets the sign up user identifier information asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <returns>Task&lt;SignUpUserIdInfo&gt;.</returns>
        public async Task<SignUpUserIdInfo> GetSignUpUserIdInfoAsync(string cellphone)
        {
            ICellphone cellphoneGrain = CellphoneFactory.GetGrain(GrainTypeHelper.GetCellphoneGrainTypeLongKey(cellphone));
            CellphoneInfo info = await cellphoneGrain.GetCellphoneInfoAsync();
            return new SignUpUserIdInfo
            {
                Cellphone = info.Cellphone,
                Registered = info.Registered,
                UserId = info.UserId
            };
        }

        /// <summary>
        ///     Gets the user information asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;UserInfo&gt;.</returns>
        public async Task<UserInfo> GetUserInfoAsync(Guid userId)
        {
            IUser user = UserFactory.GetGrain(userId);
            UserInfo info = await user.GetUserInfoAsync();
            return info.Cellphone.IsNullOrEmpty() ? null : info;
        }

        /// <summary>
        ///     Gets the withdrawalable bank card infos asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task&lt;List&lt;BankCardInfo&gt;&gt;.</returns>
        public Task<List<BankCardInfo>> GetWithdrawalableBankCardInfosAsync(Guid userId)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.GetWithdrawalableBankCardInfosAsync();
        }

        /// <summary>
        ///     Hides the bank card asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task HideBankCardAsync(HideBankCard command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.HideBankCardAsync(command);
        }

        /// <summary>
        ///     Investings the asynchronous.
        /// </summary>
        /// <param name="command">The regular investing.</param>
        /// <returns>Task.</returns>
        public Task<OrderInfo> InvestingAsync(RegularInvesting command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.InvestingAsync(command);
        }

        /// <summary>
        ///     Investings the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;JBYAccountTransactionInfo&gt;.</returns>
        public Task<JBYAccountTransactionInfo> InvestingAsync(JBYInvesting command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.InvestingAsync(command);
        }

        /// <summary>
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;ICommandHanderResult&lt;TResult&gt;&gt;.</returns>
        public Task<UserInfo> RegisterUserAsync(UserRegister command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.RegisterAsync(command);
        }

        /// <summary>
        ///     Reloads the data asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Task.</returns>
        public Task ReloadDataAsync(Guid userId)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.ReloadAsync();
        }

        /// <summary>
        ///     Resets the login password.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task.</returns>
        public Task ResetLoginPasswordAsync(ResetLoginPassword command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.ResetLoginPasswordAsync(command);
        }

        /// <summary>
        ///     Sets the payment password asynchronous.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Task.</returns>
        public Task SetPaymentPasswordAsync(SetPaymentPassword command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.SetPaymentPasswordAsync(command);
        }

        /// <summary>
        ///     Verifies the bank card asynchronous.
        /// </summary>
        /// <param name="verifyBankCardCommand">The verify bank card command.</param>
        /// <returns>Task.</returns>
        public async Task VerifyBankCardAsync(VerifyBankCard verifyBankCardCommand)
        {
            IUser user = UserFactory.GetGrain(verifyBankCardCommand.UserId);
            UserInfo userInfo = await user.GetUserInfoAsync();

            IDepositSaga saga = DepositSagaFactory.GetGrain(verifyBankCardCommand.CommandId);
            await saga.BeginProcessAsync(new DepositSagaInitData
            {
                AddBankCardCommand = null,
                AuthenticateCommand = null,
                InitUserInfo = userInfo,
                PayByLianlianCommand = null,
                PayByYilianCommand = null,
                VerifyBankCardCommand = verifyBankCardCommand
            });
        }

        /// <summary>
        ///     Withdrawals the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;SettleAccountTransactionInfo&gt;.</returns>
        public Task<SettleAccountTransactionInfo> WithdrawalAsync(Withdrawal command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.WithdrawalAsync(command);
        }

        /// <summary>
        ///     Withdrawals the asynchronous.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Task&lt;JBYAccountTransactionInfo&gt;.</returns>
        public Task<JBYAccountTransactionInfo> WithdrawalAsync(JBYWithdrawal command)
        {
            IUser user = UserFactory.GetGrain(command.UserId);
            return user.WithdrawalAsync(command);
        }

        /// <summary>
        ///     Withdrawals the resulted asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>Task&lt;SettleAccountTransactionInfo&gt;.</returns>
        public Task<SettleAccountTransactionInfo> WithdrawalResultedAsync(Guid userId, Guid transactionId)
        {
            IUser user = UserFactory.GetGrain(userId);
            return user.WithdrawalResultedAsync(transactionId);
        }

        #endregion IUserService Members
    }
}