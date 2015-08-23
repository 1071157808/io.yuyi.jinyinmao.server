using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Service.XianFeng
{
    public class TransDetail
    {
        [JsonProperty("SN")]
        public string Sn { get; set; }
        [JsonProperty("XFUSERID")]
        public string XfUserId { get; set; }
        [JsonProperty("USERID")]
        public string UserId { get; set; }
        [JsonProperty("ACC_NO")]
        public string AccNo { get; set; }
        [JsonProperty("ACC_NAME")]
        public string AccName { get; set; }
        [JsonProperty("ID_NO")]
        public string IDNo { get; set; }
        [JsonProperty("MOBILE_NO")]
        public string MobileNo { get; set; }
        [JsonProperty("PLATFORM")]
        public string Platform { get; set; }
        [JsonProperty("MERCHANTID")]
        public string MerchantId { get; set; }
        [JsonProperty("AMOUNT")]
        public string Amount { get; set; }

        [JsonProperty("BANK_NAME")]
        public string BankName { get; set; }
        [JsonProperty("BANK_CODE")]
        public string BankCode { get; set; }
        [JsonProperty("SMSCODE")]
        public string SmsCode { get; set; }
        [JsonProperty("MEMBERUSERID")]
        public string MemberUserId { get; set; }
        [JsonProperty("PAYMENTID")]
        public string Paymentid { get; set; }
        [JsonProperty("TRADENO")]
        public string TradeNo { get; set; }
        [JsonProperty("PAYCHANNEL")]
        public string PayChannel { get; set; }
        [JsonProperty("orderFlag")]
        public string OrderFlag { get; set; }

    }
}
