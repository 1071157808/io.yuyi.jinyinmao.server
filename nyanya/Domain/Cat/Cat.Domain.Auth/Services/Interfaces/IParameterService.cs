using Cat.Domain.Auth.Models;
using Domian.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.Domain.Auth.Services.Interfaces
{
    public interface IParameterService : IDomainService
    {
        Task<string> GetValue(string name);
    }
}
