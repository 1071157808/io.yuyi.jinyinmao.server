﻿// FileInformation: nyanya/Cat.Events.Products/ProductSoldOut.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:28 PM

using System;
using Domian.Events;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace Cat.Events.Products
{
    /// <summary>
    ///     产品售罄
    /// </summary>
    [Route("/ProductSoldOut")]
    public class ProductSoldOut : Event
    {
        /// <summary>
        ///     初始化<see cref="ProductSoldOut" />类的新实例. 
        /// </summary>
        public ProductSoldOut()
        {
        }

        /// <summary>
        ///     初始化<see cref="ProductSoldOut" />类的新实例. 
        /// </summary>
        /// <param name="productIdentifier">产品标识</param>
        /// <param name="sourceType">源类型</param>
        public ProductSoldOut(string productIdentifier, Type sourceType)
            : base(productIdentifier, sourceType)
        {
            this.ProductIdentifier = productIdentifier;
        }
        /// <summary>
        ///     产品标识
        /// </summary>
        public string ProductIdentifier { get; set; }
    }
    /// <summary>
    ///     产品售罄（验证）
    /// </summary>
    public class ProductSoldOutValidator : AbstractValidator<ProductSoldOut>
    {
        /// <summary>
        ///     初始化<see cref="ProductSoldOutValidator" />类的新实例. 
        /// </summary>
        public ProductSoldOutValidator()
        {
            this.RuleFor(c => c.ProductIdentifier).NotNull();
            this.RuleFor(c => c.ProductIdentifier).NotEmpty();
        }
    }
}