namespace nyanya.Cat.Models.Luckhub
{
    /// <summary>
    /// 用户抽奖活动响应
    /// </summary>
    public class UserLuckStatuResponse
    {
        /// <summary>
        ///奖品数
        /// </summary>
        public int AwardCount { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}