using System;

namespace Domain.Amp.ViewModels
{
    /// <summary>
    /// 推荐产品视图模型
    /// </summary>
    public class RecommandProductViewModel
    {
        public string BankName { get; set; }

        public Nullable<int> Duration { get; set; }

        public int Id { get; set; }

        public Nullable<int> MinNumber { get; set; }

        public string Name { get; set; }

        public string ProductIdentifier { get; set; }

        public Nullable<int> TotalNumber { get; set; }

        public Nullable<int> Unit { get; set; }

        public Nullable<decimal> Yield { get; set; }
    }
}