using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Service.XianFeng
{
    public class RequestParameter
    {
        [JsonProperty("BATCH_NO")]
        public string BatchNo { get; set; }
        [JsonProperty("MSG_SIGN")]
        public string MsgSign { get; set; }
        [JsonProperty("MSG_TYPE")]
        public string MsgType { get; set; }
        [JsonProperty("TRANS_DETAILS")]
        public List<TransDetail> TransDetails { get; set; }
        [JsonProperty("TRANS_STATE")]
        public string TransState { get; set; }
        [JsonProperty("USER_NAME")]
        public string UserName { get; set; }
        [JsonProperty("VERSION")]
        public string Version { get; set; }

    }
}
