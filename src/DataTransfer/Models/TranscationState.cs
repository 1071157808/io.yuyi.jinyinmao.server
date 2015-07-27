using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Models
{
    enum TranscationState
    {
        None = 0,
        ChongZhi = 10,
        GouMai = 20,
        BenJin = 30,
        LiXi = 40,
        QuXian = 50,
        ToJBY = 60,
        ToQianBao = 70,
        RecieveByQianBao = 80,
        RecieveByJBY = 90
    }
}
