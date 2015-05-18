using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.Domain.Users.Services.DTO
{
    /// <summary>
    /// 验证手机号是否存在返回结果
    /// </summary>
    public class CheckCellPhoneResult
    {
        /// <summary>
        /// 是否注册
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 用户类型（10金银猫 20兴业）
        /// </summary>
        public int UserType { get; set; }
    }
}
