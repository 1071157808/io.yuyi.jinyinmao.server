using System;

using System.ComponentModel;

namespace nyanya.UnitTest.Infrastructure
{
    [Description("测试实体")]
    public class TestEntity
    {
        public TestEntity()
        {
            AddDate = DateTime.Now;
            IsDeleted = false;
        }

        [Description("编号")]
        public int Id { get; set; }

        [Description("名称")]
        public string Name { get; set; }

        [Description("添加时间")]
        public DateTime AddDate { get; set; }

        [Description("是否删除")]
        public bool IsDeleted { get; set; }
    }
}
