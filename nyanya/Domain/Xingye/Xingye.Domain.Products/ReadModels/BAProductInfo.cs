using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Xingye.Domain.Products.Models;

namespace Xingye.Domain.Products.ReadModels
{
    public class BAProductInfo : ProductInfo
    {
        public string BankName { get; set; }

        public string BillNo { get; set; }

        public string BusinessLicense { get; set; }

        public string EnterpriseName { get; set; }

        public string Usage { get; set; }
    }
}