using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    /// VerifyBankCardResulted.
    /// </summary>
    [Immutable]
    public class VerifyBankCardResulted : Event
    {
        /// <summary>
        ///     Gets or sets the bank card information.
        /// </summary>
        /// <value>The bank card information.</value>
        public BankCardInfo BankCardInfo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VerifyBankCardResulted"/> is result.
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        public bool Result { get; set; }

        /// <summary>
        ///     Gets or sets the transcation desc.
        /// </summary>
        /// <value>The tran desc.</value>
        public string TranDesc { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }
    }
}