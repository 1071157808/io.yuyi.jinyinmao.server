using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.Domain.Users.Services.DTO
{
    /// <summary>
    /// ��֤�ֻ����Ƿ���ڷ��ؽ��
    /// </summary>
    public class CheckCellPhoneResult
    {
        /// <summary>
        /// �Ƿ�ע��
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// �û����ͣ�10����è 20��ҵ��
        /// </summary>
        public int UserType { get; set; }
    }
}
