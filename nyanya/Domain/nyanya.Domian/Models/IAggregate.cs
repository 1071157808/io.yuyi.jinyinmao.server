﻿// FileInformation: nyanya/Domian/IAggregate.cs
// CreatedTime: 2014/07/06   12:09 AM
// LastUpdatedTime: 2014/07/06   2:06 AM

namespace Domian.Models
{
    internal interface IAggregate
    {
        // 基本原则 => 同一事务中只能修改一个聚合

        // 原则一 => 一致性
        // 聚合具有事务一致性，即要求立即性和原子性；
        // 聚合表达了与事务一致性边界相同的意思；
        // 在一个聚合内，往往使用单聚合；
        // 同时在一个事务中只修改一个聚合实例；
        // 每次客户请求应该只在一个聚合实例上执行一个命令方法。

        // 原则二 => 设计小聚合
        // 使用根实体来表示聚合，其中只包含最小数量的属性或值类型属性。
        // 只有那么必须与其他属性保持一致的属性才是所需的属性。
        // 大量的聚合都可以建模成单个实体——根实体。

        // 原则三 => 聚合中多使用值对象而非实体
        // 将聚合的内部建模成值对象有很多好处。根据你所选用的持久化机制，值对象可以随着根实体而序列化，而实体则需要单独的存储区域予以跟踪。

        // 原则四 => 通过唯一标识引用其他聚合
        // 即最好不要直接引用外部聚合，而是通过保存外部聚合的全局唯一标识来引用外部聚合

        // 原则五 => 在边界之外使用最终一致性
        // 任何跨聚合的业务规则都不能总是保持处于最新状态。通过事件处理、批处理或者其他更新机制，我们可以在一定时间之内处理好他方依赖。[Evans,p.128]
        // 在一个大规模、高吞吐量的企业系统中，要使所有的聚合实例一致是不可能的。认识到这一点，便知道在较小规模的系统中使用最终一致性也是有必要的。
        // 对于同步延迟，可以使用推送的方式更新用户界面。但是这往往具有一定的技术难度，最好也最简单的方法就是在界面上直接告诉用户，此时的状态是不正确的。
        // 延迟是客观存在的，而开发人员则总是期待着原子性操作。
        // 在DDD中，有一种很实用的方法可以支持最终一致性，即一个聚合的命令方法所发布的领域事件及时地发送给异步的订阅方。

        // 原则六 => 如果应该由执行该用例的用户来保证数据的一致性，使用事务一致性。
        //          如果是需要其他用户或者系统来保证数据一致性，则使用最终一致性。
        // 最终一致性所需要的技术机制：消息，定时器，或者后台线程。
    }
}