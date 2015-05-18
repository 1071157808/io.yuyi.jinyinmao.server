// FileInformation: nyanya/Xingye.Domain.Auth/UseVeriCodeResult.cs
// CreatedTime: 2014/07/06   4:40 PM
// LastUpdatedTime: 2014/07/09   2:10 AM

namespace Xingye.Domain.Auth.Services.DTO
{
    public class UseVeriCodeResult
    {
        public string Cellphone { get; set; }

        public bool Result { get; set; }

        public static UseVeriCodeResult CreateFailedResult()
        {
            return new UseVeriCodeResult { Result = false };
        }
    }
}