// FileInformation: nyanya/Domian/IValueObject.cs
// CreatedTime: 2014/07/05   4:25 PM
// LastUpdatedTime: 2014/07/05   5:26 PM

namespace Domian.Models
{
    public interface IValueObject
    {
        // Value objects capture the state of other (entity) objects. What
        // is interesting about them is not their identity but the value
        // that they represent. Classic examples are numbers, strings, and
        // dates (think: number 5, “Bloggs,” or 1-May-2009).
        // Value objects should be immutable and often have a closed
        // set of operations that define an “algebra” for the type.

        // 度量或者描述了领域中的一件东西。而不应该成为你领域中的一件东西。
        // 作为不变量。在创建后，其属性就不可以改变
        // 将不同的相关的属性组合成一个概念整体
        // 当度量和描述改变时，可以用另一个值对象予以替换
        // 可以和其他值对象进行相等性比较
        // 不会对协作对象造成副作用

        // 如果试图将多个属性加在一个实体上，但这样弱化了各个属性之间的关系，那么此时便应该考虑将这些相互关联的属性
        // 组合在一个值对象中了。每个值对象都是一个内聚的概念整体，它表达了通用语言中的一个概念。如果其中一个属性表
        // 达了一种描述性概念，那么我们应该把与该概念相关的所有属性集中起来。如果其中一个或者多个属性发生了改变，那
        // 么可以考虑对整体值对象进行替换。

        // 如果概念不需要唯一标识，那么请将其建模成一个值对象

        // 值对象中只应该有无副作用行为（无副作用函数），它只用于产生输出，而不会修改对象的状态

        // 值对象不仅仅是一个属性容器，更强大的特性在于其拥有的无副作用函数

        // 如果一个值对象方案将一个实体对象作为参数时，最好的方式是，让实体对象使用该方案的返回结果来修改其自身的状态

        // 值对象的不变性：可以通过将所有的属性设置为只读的，并且其对应的字段也全部使用只读的字段

        // 对于不变的值对象来说，在不同的实例间共享属性是不会出现什么问题的

        // 值对象的无副作用方法的名字是重要的，一般不需要使用get前缀，这种方式使得代码与通用语言保持一致
    }
}