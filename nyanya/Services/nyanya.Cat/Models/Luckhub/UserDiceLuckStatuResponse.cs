using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyanya.Cat.Models.Luckhub
{
    /// <summary>
    ///
    /// </summary>
    public class BuildDiceLuckRecordResponse : BuildLuckRecordResponse
    {
        public int UserDiceNum { get; set; }

        public int ServerDiceNum { get; set; }
    }
}