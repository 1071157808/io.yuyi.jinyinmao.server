using Domian.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.Domain.Auth.Models
{
    public class Parameter : IAggregateRoot
    {
        public Parameter() { }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
