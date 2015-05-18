// FileInformation: nyanya/Cat.Events.Yilian/IYilianResultEvent.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:28 PM

namespace Cat.Events.Yilian
{
    /// <summary>
    ///     易联结果接口
    /// </summary>
    public interface IYilianResultEvent
    {
        /// <summary>
        ///     消息
        /// </summary>
        string Message { get; set; }
        /// <summary>
        ///     结果
        /// </summary>
        bool Result { get; set; }
        /// <summary>
        ///     订单编号
        /// </summary>
        string SequenceNo { get; set; }
        /// <summary>
        ///     用户标识
        /// </summary>
        string UserIdentifier { get; set; }
    }
}