using System;
using System.Threading.Tasks;
using Infrastructure.SMS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace nyanya.Infrastructure.SMS.Tests
{
    [TestClass]
    public class SmsServiceTests
    {
        private readonly SmsService _smsService = new SmsService();

        private const string Cellphone = "13916203671";

        private const string SmsContent1 = "购买成功，订单号：ONBJ2R51384095，订单金额1000000元。如需帮助，请致电400855633";
        private const string SmsContent2 = "购买失败，订单号：ODBJ3H48435220，订单金额15000元。失败原因：支付失败，原因，您卡上的余额不足。如需帮助 4008556333";

        [TestMethod]
        public void SendAsyncTest()
        {
            //var result = _smsService.SendAsync(Cellphone, SmsContent2, 0);
            //Assert.IsTrue(result.Result);


            var resultXy = _smsService.SendAsync(Cellphone, SmsContent2, 1);
            Assert.IsTrue(resultXy.Result); 
        }
    }
}
