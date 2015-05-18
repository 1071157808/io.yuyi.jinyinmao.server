namespace Cat.Events.Users
{
    /// <summary>
    ///     添加银行卡接口
    /// </summary>
    public interface IAddedABankCard
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
        ///     用户标识
        /// </summary>
        string UserIdentifier { get; }
    }
}