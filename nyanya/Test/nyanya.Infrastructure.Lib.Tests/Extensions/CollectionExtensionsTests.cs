using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Infrastructure.Lib.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Infrastructure.Lib.Extensions;
using nyanya.UnitTest.Infrastructure;

namespace Infrastructure.Lib.Tests.Extensions
{
    [TestClass]
    public class CollectionExtensionsTests
    {
        [TestMethod()]
        public void WhereIfTest_IQueryable()
        {
            IQueryable<int> source = new List<int> { 1, 2, 3, 4, 5, 6, 7 }.AsQueryable();
            CollectionAssert.AreEqual(source.WhereIf(m => m > 5, false).ToList(), source.ToList());

            List<int> actual = new List<int> { 6, 7 };
            CollectionAssert.AreEqual(source.WhereIf(m => m > 5, true).ToList(), actual);
        }

        [TestMethod()]
        public void OrderByTest_IQueryable()
        {
            IQueryable<TestEntity> source = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "abc" },
                new TestEntity { Id = 4, Name = "fda", IsDeleted = true },
                new TestEntity { Id = 6, Name = "rwg", IsDeleted = true },
                  new TestEntity { Id = 6, Name = "swg", IsDeleted = true },
                new TestEntity { Id = 3, Name = "hdg" },
            }.AsQueryable();


            var sortConditions = new List<SortCondition>
            {
                new SortCondition("Id", ListSortDirection.Descending),
                new SortCondition("Name", ListSortDirection.Descending)
            };

            Assert.AreEqual(source.OrderBy("Id").ToArray()[1].Name, "hdg");
            Assert.AreEqual(source.OrderBy("Name", ListSortDirection.Descending).ToArray()[3].Id, 4);
            Assert.AreEqual(source.OrderBy(new SortCondition("Id")).ToArray()[1].Name, "hdg");
            Assert.AreEqual(source.OrderBy(new SortCondition("Name", ListSortDirection.Descending)).ToArray()[3].Id, 4);
            Assert.AreEqual(source.OrderBy(sortConditions.ToArray()).ToArray()[0].Name,"swg");
        }

        [TestMethod()]
        public void ThenByTest_IQueryable()
        {
            IQueryable<TestEntity> source = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "abc" },
                new TestEntity { Id = 4, Name = "fda", IsDeleted = true },
                new TestEntity { Id = 6, Name = "rwg", IsDeleted = true },
                new TestEntity { Id = 3, Name = "hdg" },
            }.AsQueryable();
            Assert.AreEqual(source.OrderBy("IsDeleted").ThenBy("Id").ToArray()[2].Name, "fda");
            Assert.AreEqual(source.OrderBy("IsDeleted", ListSortDirection.Descending).ThenBy("Id", ListSortDirection.Descending).ToArray()[2].Name,
                "hdg");
        }
       
    }
}
