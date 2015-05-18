using System;
using Infrastructure.Lib.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Infrastructure.Lib.Tests.Extensions
{
    [TestClass]
    public class EnumExtensionsTests
    {
        [TestMethod()]
        public void ToDescriptionTest()
        {
            var value = TestEnum.EnumItemA;
            Assert.AreEqual(value.ToDescription(), "枚举项A");

            value = TestEnum.EnumItemB;
            Assert.AreEqual(value.ToDescription(), "EnumItemB");
        }

        [TestMethod()]
        public void IsDefinedTest()
        {
            const TestEnum value1 = (TestEnum)1;
            const TestEnum value2 = (TestEnum)2;

            Assert.AreEqual(value1.IsDefined(),true);
            Assert.AreEqual(value2.IsDefined(), false);
        }
    }
    internal enum TestEnum
    {
        [System.ComponentModel.Description("枚举项A")]
        EnumItemA = 0,

        EnumItemB = 1
    }
}
