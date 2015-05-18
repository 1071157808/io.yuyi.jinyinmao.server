// FileInformation: nyanya/Infrastructure.SMS/SmsService.cs
// CreatedTime: 2014/08/06   2:40 PM
// LastUpdatedTime: 2014/08/14   1:00 AM

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Infrastructure.SMS
{
    public static class SmsPrefixHelper
    {
        public static string GetSmsPrefix(int type)
        {
            switch (type)
            {
                case 0:
                    return "【金银猫】";

                case 1:
                    return "【兴业票】";

                default:
                    return "【金银猫】";
            }
        }
    }
    public class SmsService : ISmsService, ISmsAlertsService
    {
        private static readonly string smsEnable = ConfigurationManager.AppSettings.Get("SmsEnable") ?? "True";
        private static readonly string smsGateway = ConfigurationManager.AppSettings.Get("SmsGateway") ?? "http://sms.i.jinyinmao.com.cn/sms/cmcc/send_msg.json";
        private const string Signs = "，、。：“”";

        private bool SmsEnable
        {
            get { return smsEnable.ToUpper() == "TRUE"; }
        }
        #region ISmsAlertsService Members

        public async Task<bool> AlertAsync(string content)
        {
            if (!this.SmsEnable)
                return true;

            return true;
        }

        #endregion ISmsAlertsService Members

        #region ISmsService Members

        public async Task<bool> SendAsync(string cellphone, string content, int type = 0)
        {
            if (this.SmsEnable)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        foreach (var subContent in GetMsgList(content, type))
                        {
                            var data = new FormUrlEncodedContent(new Dictionary<string, string>()
                            {
                                {"name", "berek"},
                                {"password", "berekqu"},
                                {"mobs", cellphone},
                                {"msg", subContent},
                                {"type", type.ToString()}
                            });

                            HttpResponseMessage response = await client.PostAsync(smsGateway, data);

                            string ret = await response.Content.ReadAsStringAsync();
                            dynamic stuff = JObject.Parse(ret);
                            string status = stuff.cat.status;
                            if (status != "0")
                                return false;
                        }
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return true;
        }

        public List<string> GetMsgList(string content, int type)
        {
            var msgArray = new List<string>();

            var cutLenght = 70 - SmsPrefixHelper.GetSmsPrefix(type).Length;
            var subBegin = 0;

            for (var i = 0; i < content.Length; i++)
            {
                if (i % cutLenght != 0) continue;

                string cutContent;
                int tmpLenght = cutLenght;

                if (subBegin + tmpLenght >= content.Length)
                {
                    cutContent = content.Substring(subBegin, content.Length - subBegin);
                }
                else
                {
                    cutContent = content.Substring(subBegin, tmpLenght);
                    var index = cutContent.LastIndexOfAny(Signs.ToCharArray());
                    if (index != -1)
                    {
                        tmpLenght = index + 1;
                        cutContent = content.Substring(subBegin, tmpLenght);
                    }
                }

                msgArray.Add(cutContent);

                subBegin += tmpLenght;
            }

            return msgArray;
        }
        #endregion ISmsService Members
    }
}
