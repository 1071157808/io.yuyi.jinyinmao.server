namespace nyanya.Meow.Models
{
    /// <summary>
    /// 用户活动响应
    /// </summary>
    public class UserActivityResponse
    {
        /// <summary>
        /// 活动状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 额外收益
        /// </summary>
        public decimal ExtraInterest { get; set; }

        public int MiniCash { get; set; }
    }
}