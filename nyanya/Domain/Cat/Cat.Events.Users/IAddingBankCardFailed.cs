namespace Cat.Events.Users
{
    /// <summary>
    ///     添加银行卡失败接口
    /// </summary>
    public interface IAddingBankCardFailed
    {
        /// <summary>
        ///     银行卡号
        /// </summary>
        string BankCardNo { get; }
        /// <summary>
        ///     银行名称
        /// </summary>
        string BankName { get; }
        /// <summary>
        ///     手机号
        /// </summary>
        string Cellphone { get; }
        /// <summary>
        ///     添加银行卡失败消息
        /// </summary>
        string Message { get; }
        /// <summary>
        ///     用户标识
        /// </summary>
        string UserIdentifier { get; }
    }
}