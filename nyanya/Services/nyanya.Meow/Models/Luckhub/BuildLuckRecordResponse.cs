namespace nyanya.Meow.Models.Luckhub
{
    /// <summary>
    /// 生成抽奖记录响应
    /// </summary>
    public class BuildLuckRecordResponse
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 奖品等级
        /// </summary>
        public int AwardLevel { get; set; }
    }
}