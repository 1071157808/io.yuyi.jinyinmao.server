using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Infrastructure.Lib.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nyanya.UnitTest.Infrastructure;

namespace Infrastructure.Lib.Tests.Extensions
{
    [TestClass]
    public class TypeExtensionsTests
    {
        [TestMethod()]
        public void IsNullableTypeTest()
        {
            // ReSharper disable ConvertNullableToShortForm
            Assert.IsTrue(typeof(int?).IsNullableType());
            Assert.IsTrue(typeof(Nullable<int>).IsNullableType());

            Assert.IsFalse(typeof(int).IsNullableType());
        }

        [TestMethod()]
        public void IsEnumerableTest()
        {
            Assert.IsTrue(typeof(string[]).IsEnumerable());
            Assert.IsTrue(typeof(ICollection<string>).IsEnumerable());
            Assert.IsTrue(typeof(IEnumerable<string>).IsEnumerable());
            Assert.IsTrue(typeof(IList<string>).IsEnumerable());
            Assert.IsTrue(typeof(Hashtable).IsEnumerable());
            Assert.IsTrue(typeof(HashSet<string>).IsEnumerable());

            Assert.IsFalse(typeof(int).IsEnumerable());
            Assert.IsFalse(typeof(string).IsEnumerable());
        }

        [TestMethod()]
        public void GetNonNummableType()
        {
            Assert.AreEqual(typeof(int?).GetNonNummableType(), typeof(int));
            Assert.AreEqual(typeof(Nullable<int>).GetNonNummableType(), typeof(int));

            Assert.AreEqual(typeof(int).GetNonNummableType(), typeof(int));
        }

        [TestMethod()]
        public void GetUnNullableTypeTest()
        {
            Assert.AreEqual(typeof(int?).GetUnNullableType(), typeof(int));
            Assert.AreEqual(typeof(Nullable<int>).GetUnNullableType(), typeof(int));

            Assert.AreEqual(typeof(int).GetUnNullableType(), typeof(int));
        }

        [TestMethod()]
        public void ToDescriptionTest()
        {
            Type type = typeof(TestEntity);
            Assert.AreEqual(type.ToDescription(), "测试实体");
            PropertyInfo property = type.GetProperty("Id");
            Assert.AreEqual(property.ToDescription(), "编号");

            type = GetType();
            Assert.AreEqual(type.ToDescription(), "TypeExtensionsTests");
        }

        [TestMethod()]
        public void AttributeExistsTest()
        {
            Type type = GetType();
            Assert.IsTrue(type.AttributeExists<TestClassAttribute>());
            MethodInfo method = type.GetMethod("AttributeExistsTest");
            Assert.IsTrue(method.AttributeExists<TestMethodAttribute>());
        }

        [TestMethod()]
        public void GetAttributesTest()
        {
            Type type = GetType();
            Assert.AreEqual(type.GetAttributes<DescriptionAttribute>().Length, 0);
            Assert.AreEqual(type.GetAttributes<TestClassAttribute>().Length, 1);
        }

    }
}
