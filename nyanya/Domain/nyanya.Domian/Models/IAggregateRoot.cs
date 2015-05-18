// FileInformation: nyanya/Domian/IAggregateRoot.cs
// CreatedTime: 2014/07/14   4:42 PM
// LastUpdatedTime: 2014/07/28   12:23 AM

namespace Domian.Models
{
    public interface IAggregateRoot : IEntity
    {
        // 一般将实体建模成聚合根。
        // 每个聚合根必须拥有一个全局的唯一标识，往往是GUID。同时保存用于乐观并发的版本号。
        // 尽量将根实体所包含的其他聚合建模成值对象，即只保存唯一标识。

        // 在聚合根中使用工厂方法创建其他实体或者值对象。
    }
}