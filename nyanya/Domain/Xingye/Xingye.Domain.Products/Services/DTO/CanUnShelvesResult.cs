using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xingye.Domain.Products.Services.DTO
{
    public class CanUnShelvesResult
    {
        public string ProductIdentifier { get; set; }

        public string ProductNo { get; set; }

        public bool Result { get; set; }
    }
}