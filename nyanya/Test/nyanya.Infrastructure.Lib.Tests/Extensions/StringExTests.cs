using Infrastructure.Lib.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace nyanya.Infrastructure.Lib.Tests.Extensions
{
    [TestClass]
    public class StringExTests
    {
        [TestMethod]
        public void TestHidenString()
        {
            string test1 = "";
            string test2 = "test";
            string test3 = "testtest";
            string test4 = "testtesttest";

            Assert.AreEqual(test1.HideStringBalance(), "", false);
            Assert.AreEqual(test2.HideStringBalance(), "test", false);
            Assert.AreEqual(test3.HideStringBalance(), "****test", false);
            Assert.AreEqual(test4.HideStringBalance(), "test****test", false);
            Assert.AreEqual(test4.HideStringBalance(-5), "", false);
            Assert.AreEqual(test4.HideStringBalance(5), "*******ttest", false);
            Assert.AreEqual(test4.HideStringBalance(3), "tes******est", false);
            Assert.AreEqual(test4.HideStringBalance(int.MaxValue / 2), "testtesttest", false);
        }
    }
}