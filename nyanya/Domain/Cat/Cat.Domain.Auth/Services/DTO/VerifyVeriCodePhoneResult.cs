using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.Domain.Auth.Services.DTO
{
    public class VerifyVeriCodePhoneResult
    {
        public bool Successful { get; set; }

        public string Token { get; set; }
    }
}
