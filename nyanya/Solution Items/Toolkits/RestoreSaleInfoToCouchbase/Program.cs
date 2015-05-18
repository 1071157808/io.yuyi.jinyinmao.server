// FileInformation: nyanya/RestoreSaleInfoToCouchbase/Program.cs
// CreatedTime: 2014/09/10   11:23 AM
// LastUpdatedTime: 2014/09/10   12:12 PM

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Cat.Domain.Products.Database;
using Cat.Domain.Products.Models;
using Infrastructure.Cache.Couchbase;

namespace RestoreSaleInfoToCouchbase
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                int n = 100;
                int i = 0;
                while (true)
                {
                    List<Product> products;
                    using (ProductContext db = new ProductContext())
                    {
                        products = db.ReadonlyQuery<Product>().Include(p => p.SaleInfo).OrderBy(p => p.ProductIdentifier).Skip(n * i).Take(n).ToList();
                    }

                    foreach (Product product in products)
                    {
                        ProductShareCacheModel model = ProductShareCache.GetShareCache(product.ProductIdentifier);
                        if (model == null || (model.Available == 0 && model.Paid == 0 && model.Paying == 0 && model.Sum == 0))
                        {
                            if (product.SoldOut)
                            {
                                if (ProductShareCache.RestoreSoldOutShareCache(product.ProductIdentifier, product.SaleInfo.FinancingSumCount))
                                {
                                    Console.WriteLine(product.ProductIdentifier + "     OK");
                                }
                                else
                                {
                                    Console.WriteLine(product.ProductIdentifier + "     Error");
                                }
                            }
                            else
                            {
                                Console.WriteLine(product.ProductIdentifier + "     Fatal");
                            }
                        }
                        else
                        {
                            Console.WriteLine(product.ProductIdentifier + "     PASS");
                        }
                    }

                    if (products.Count == 0)
                    {
                        break;
                    }
                    Console.WriteLine("========================     " + i);
                    i++;
                }
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}