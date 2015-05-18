// FileInformation: nyanya/Cat/CatHelper.cs
// CreatedTime: 2014/09/03   9:43 AM
// LastUpdatedTime: 2014/09/03   5:39 PM

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using Cat;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using HttpCookie = System.Web.HttpCookie;

// ReSharper disable InconsistentNaming

public static class CatHelper
{
    #region Public Fields

    public const string addCardTryPrice = "1.08";
    public const bool IsXYFlag = true;
    public const bool IsFDFlag = true;

    #endregion Public Fields

    #region Private Fields

    private static readonly string ApiHost = ConfigurationManager.AppSettings.Get("ApiHost");
    private static readonly string ApiHostPort = ConfigurationManager.AppSettings.Get("ApiHostPort");
    private static readonly string ApiPath = ConfigurationManager.AppSettings.Get("ApiPath");
    private static readonly string DebugEnable = ConfigurationManager.AppSettings.Get("Debug");
    private static readonly string HostName = HttpContext.Current.Request.Url.Host.ToString();

    #endregion Private Fields

    #region Public Methods

    public static bool IsProduct(){
        return HostName == "www.jinyinmao.com.cn" || HostName == "jinyinmao.com.cn";
    }

    public static bool IsTest(){
        return HostName == "www.test.ad.jinyinmao.com.cn";
    }

    public static bool IsDev(){
        return !IsProduct() && !IsTest();
    }

    public static string GetXYDetailUrl(object productId){
        if(IsProduct()){
            return string.Format("https://piao.cib.com.cn/amp/detail?id={0}", productId.ToString());
        } else {
            return string.Format("https://xy.test.jymtest.com/amp/detail?id={0}", productId.ToString());
        }
    }

    public static string MD5(object md5Key){
        byte[] result = Encoding.Default.GetBytes(md5Key.ToString());
        MD5 md5 = new MD5CryptoServiceProvider();  
        byte[] output = md5.ComputeHash(result);  
        return BitConverter.ToString(output).Replace("-","");
    }

    public static int ConvertInt(object count, object minCount, object maxCount)
    {
        int iCount = count.ToIntSafely(1);
        int aCount = minCount.ToIntSafely();
        int bCount = maxCount.ToIntSafely(1);

        if (iCount > bCount)
        {
            return bCount;
        }

        if (iCount < aCount)
        {
            return aCount;
        }

        return iCount;
    }

    public static string FixBankCard(object card)
    {
        string bankCard = card.ToString();
        return bankCard.GetLast(4);
    }

    public static string FixCellphone(object cellphone)
    {
        string str = cellphone.ToString();
        string start = str.GetFirst(3);
        string end = str.GetLast(4);
        return string.Format("{0}****{1}", start, end);
    }

    public static string GetBankCode(string bankName)
    {
        switch (bankName)
        {
            case "工商银行":
                return "bank-c01";

            case "招商银行":
                return "bank-c02";

            case "广发银行":
                return "bank-c03";

            case "华夏银行":
                return "bank-c04";

            case "海南农信社":
                return "bank-c05";

            case "广州银行":
                return "bank-c06";

            case "中信银行":
                return "bank-c07";

            case "兴业银行":
                return "bank-c08";

            case "浦发银行":
                return "bank-c09";

            case "深发银行":
                return "bank-c10";

            case "建设银行":
                return "bank-c11";

            case "广州农商行":
                return "bank-c12";

            case "农业银行":
                return "bank-c13";

            case "邮储银行":
                return "bank-c14";

            case "光大银行":
                return "bank-c15";

            case "平安银行":
                return "bank-c16";

            case "民生银行":
                return "bank-c17";

            case "中国银行":
                return "bank-c18";
        }
        return "";
    }

    public static string GetBankCode(object BankName)
    {
        return GetBankCode(BankName.ToString());
    }

    public static string GetGuaranteeMode(object GMode){
        int iMode = GMode.ToIntSafely();
        switch(iMode){
            case 10: return "银行保兑";
            case 20: return "央企担保";
            case 30: return "国企担保";
            case 40: return "国有担保公司担保";
            case 50: return "担保公司担保";
            case 60: return "上市集团担保";
            case 70: return "集团担保";
            case 80: return "国资参股担保公司担保";
        }
        return "";
    }

    public static NyanyaWebData GetData(this HttpRequestBase request, string route)
    {
        NyanyaWebData data;
        HttpResponseMessage responseMessage = null;
        try
        {
            if (request.Url == null)
            {
                throw new ArgumentException("Request Url is null");
            }

            if (route.Substring(0, 1) == "/")
            {
                route = route.Remove(0, 1);
            }
            string NewApiPath = ApiPath;
            if (ApiPath.Substring(0, 1) == "/")
            {
                NewApiPath = ApiPath.Remove(0, 1);
            }
            Uri baseUri = new Uri("https://" + ApiHost + ApiHostPort);
            CookieContainer cookieContainer = new CookieContainer();
            cookieContainer.Add(request.Cookies.ToCookieCollection(ApiHost));
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
            RestClient client = new RestClient(baseUri.ToString());
            RestRequest restRequest = new RestRequest(NewApiPath + route, Method.GET);
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddHeader("Content-Type", "application/json");
            client.CookieContainer = cookieContainer;
            IRestResponse response = client.Execute(restRequest);
            dynamic date = JsonConvert.DeserializeObject(response.Content);
            responseMessage = new HttpResponseMessage(response.StatusCode);
            responseMessage.Content = new StringContent(JsonConvert.SerializeObject(response.Content));
            responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            string message = responseMessage.IsSuccessStatusCode ? response.ErrorMessage : date.Message;
            data = new NyanyaWebData
            {
                Data = date,
                Message = message,
                Response = responseMessage,
                Result = true,
                Status = Convert.ToInt32(responseMessage.StatusCode)
            };
        }
        catch (Exception e)
        {
            data = new NyanyaWebData
            {
                Data = JObject.Parse(JsonConvert.SerializeObject(new { e.Message })),
                Message = e.Message,
                Response = responseMessage,
                Result = false,
                Status = 500
            };
        }

        if (DebugEnable != null && DebugEnable.ToUpper() == "T" && !string.IsNullOrEmpty(request.QueryString["debug"]))
        {
            if (request.QueryString["debug"] == "1")
            {
                HttpContext.Current.Response.Write(string.Format("<!--{0}|{1}-->", route, JsonConvert.SerializeObject(data)));
            }
            else if (request.QueryString["debug"] == "2")
            {
                HttpContext.Current.Response.Write(string.Format("<!--{0}|{1}-->", route, JsonConvert.SerializeObject(new { data.Data, data.Message, data.Result, data.Status })));
            }
            else
            {
                HttpContext.Current.Response.Write(string.Format("<!--{0}|{1}-->", route, JsonConvert.SerializeObject(data.Data)));
            }
        }

        return data;
    }

    public static NyanyaWebData[] GetDatas(this HttpRequestBase request, params string[] routes)
    {
        string BatchRoute = "$batch";
        List<NyanyaWebData> ListData = new List<NyanyaWebData>();
        if (request.Url == null)
        {
            throw new ArgumentException("Request Url is null");
        }
        string NewApiPath = ApiPath;
        if (ApiPath.Substring(0, 1) == "/")
        {
            NewApiPath = ApiPath.Remove(0, 1);
        }
        CookieContainer cookieContainer = new CookieContainer();
        cookieContainer.Add(request.Cookies.ToCookieCollection(ApiHost));
        ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
        RestClient client = new RestClient(new Uri("https://" + ApiHost + ApiHostPort).ToString());
        RestRequest BatchRequest = new RestRequest(NewApiPath + BatchRoute, Method.POST);
        BatchRequest.RequestFormat = DataFormat.Json;
        BatchRequest.AddHeader("Content-Type", "application/json");
        List<object> Body = new List<object>();
        foreach (string i in routes)
        {
            string Route = i;
            if (Route.Substring(0, 1) == "/")
            {
                Route = Route.Remove(0, 1);
            }
            Body.Add(new { Method = "GET", RelativeUrl = NewApiPath + Route });
        }
        BatchRequest.AddBody(Body.ToArray());
        client.CookieContainer = cookieContainer;
        IRestResponse BatchResponse = client.Execute(BatchRequest);
        if (BatchResponse.StatusCode != HttpStatusCode.OK)
        {
            return ListData.ToArray();
        }
        IList<dynamic> DataL = JsonConvert.DeserializeObject<IList<dynamic>>(BatchResponse.Content);
        foreach (dynamic i in DataL)
        {
            HttpResponseMessage Res = new HttpResponseMessage((HttpStatusCode)i.Code);
            Res.Content = new StringContent(JsonConvert.SerializeObject(i));
            Res.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            string message = Res.IsSuccessStatusCode ? "OK" : JsonConvert.DeserializeObject(i.Body.ToString()).Message;
            dynamic Rdata = Res.IsSuccessStatusCode ? JsonConvert.DeserializeObject(i.Body.ToString()) : null;
            NyanyaWebData d = new NyanyaWebData
            {
                Data = Rdata,
                Message = message,
                Response = Res,
                Result = true,
                Status = int.Parse(i.Code.ToString())
            };
            ListData.Add(d);
        }

        if (DebugEnable != null && DebugEnable.ToUpper() == "T" && !string.IsNullOrEmpty(request.QueryString["debug"]))
        {
            foreach (NyanyaWebData data in ListData)
            {
                if (request.QueryString["debug"] == "1")
                {
                    HttpContext.Current.Response.Write(string.Format("<!--{0}-->", JsonConvert.SerializeObject(data)));
                }
                else if (request.QueryString["debug"] == "2")
                {
                    HttpContext.Current.Response.Write(string.Format("<!--{0}-->", JsonConvert.SerializeObject(new { data.Data, data.Message, data.Result, data.Status })));
                }
                else
                {
                    HttpContext.Current.Response.Write(string.Format("<!--{0}-->", JsonConvert.SerializeObject(data.Data)));
                }
            }
        }

        return ListData.ToArray();
    }

    public static string GetIdCardName(object idCardType)
    {
        int iType = idCardType.ToIntSafely();
        switch (iType)
        {
            case 0:
                return "身份证";

            case 1:
                return "护照";

            case 2:
                return "台湾同胞来往内地通行证";

            case 3:
                return "军人证";
        }
        return "身份证";
    }

    public static string GetLoginUrl(string url = "")
    {
        string baseUrl = "~/passport/login";
        if (!string.IsNullOrEmpty(url))
        {
            baseUrl = baseUrl + "?backUrl=" + HttpUtility.UrlEncode(url);
        }
        return baseUrl;
    }

    public static string GetOrderShowingStatus(object showingStatus)
    {
        int status = showingStatus.ToIntSafely();
        switch (status)
        {
            case 10:
                return "付款中";

            case 20:
                return "待起息";

            case 30:
                return "已起息";

            case 40:
                return "已结息";

            case 50:
                return "支付失败";
        }
        return "异常";
    }

    public static string GetProductName(object prdName, object prdNumber)
    {
        int iNumber = prdNumber.ToIntSafely(-1);
        if (iNumber == -1)
        {
            return prdName.ToString();
        }
        return string.Format("{0}第{1}期", prdName, prdNumber);
    }

    public static string GetProductShowingStatus(object statusObject)
    {
        int status = statusObject.ToIntSafely();
        if (status == 30)
        {
            return "amp-01";
        }
        if (status < 30)
        {
            return "amp-02";
        }
        if (status == 40)
        {
            return "amp-03";
        }
        if (status == 50)
        {
            return "amp-04";
        }
        return "amp-02";
    }

    public static string GetValueDate_detail(dynamic data)
    {
        string result = "";
        try
        {
            if (data.Data.ValueDateMode < 30)
            {
                result = data.Data.ValueDateString;
            }
            else if (data.Data.ValueDateMode == 30)
            {
                result = data.Data.ValueDate.ToString("yyyy-MM-dd");
            }
        }
        catch
        {
            result = "";
        }
        return result;
    }

    public static string GetValueDate_list(dynamic item)
    {
        string result = "";
        try
        {
            if (item.ValueDateMode < 30)
            {
                result = string.Format("({0})", item.ValueDateString);
            }
            else if (item.ValueDateMode == 30)
            {
                result = string.Format("(起息日：{0})", item.ValueDate.ToString("yyyy-MM-dd"));
            }
        }
        catch
        {
            result = "";
        }
        return result;
    }

    public static string GetValueDate_Note(dynamic item)
    {
        string result = "";
        try
        {
            if (item.ValueDateMode < 30)
            {
                result = item.ValueDateString;
            }
            else if (item.ValueDateMode == 30)
            {
                result = string.Format("起息日:{0}", item.ValueDate.ToString("yyyy-MM-dd"));
            }
        }
        catch
        {
            result = "";
        }
        return result;
    }

    public static string GetYieldString(object value)
    {
        decimal d;
        try
        {
            d = Convert.ToDecimal(value);
        }
        catch
        {
            return "0";
        }
        d.RoundScale(1, 2);
        return d.ToString();
    }

    public static void GotoLogin(string url = "")
    {
        if (string.IsNullOrEmpty(url))
        {
            Uri uri = HttpContext.Current.Request.Url;
            url = uri.AbsoluteUri;
        }
        string loginUrl = GetLoginUrl(url);
        HttpContext.Current.Response.Redirect(loginUrl);
    }

    public static bool IsDataSuccess(dynamic Data)
    {
        bool successful = false;
        try
        {
            successful = Data.Result == true && Data.Status <= 300 && Data.Status >= 200;
        }
            // ReSharper disable once EmptyGeneralCatchClause
        catch
        {
            //ignore
        }
        return successful;
    }

    public static string JsWrapper(string js)
    {
        return js.Replace("<script src='", "'").Replace(".js'></script>", "'");
    }

    public static string Request(string name, string defVal = "")
    {
        if (HttpContext.Current.Request.QueryString[name] != null)
        {
            return HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString[name]);
        }
        return defVal;
    }

    public static string RequestForm(string name, string defVal = "")
    {
        if (HttpContext.Current.Request.Form[name] != null)
        {
            return HttpContext.Current.Request.Form[name];
        }
        return defVal;
    }

    public static bool SignInCheck(dynamic userInfo, string backUrl = "")
    {
        if (!IsDataSuccess(userInfo))
        {
            GotoLogin(backUrl);
            return false;
        }
        return true;
    }

    public static decimal SyPrice(decimal price, decimal yield, decimal period, decimal year = 360.00m)
    {
        return Math.Floor(price * yield * period / year) / 100;
    }

    public static decimal SyPrice(object price, object yield, object period)
    {
        decimal d = SyPrice(Convert.ToDecimal(price), Convert.ToDecimal(yield), Convert.ToDecimal(period));
        return decimal.Round(d, 2, MidpointRounding.AwayFromZero);
    }

    public static CookieCollection ToCookieCollection(this HttpCookieCollection cookies, string domain)
    {
        CookieCollection collection = new CookieCollection();
        for (int j = 0; j < cookies.Count; j++)
        {
            HttpCookie cookie = cookies.Get(j);
            Cookie oC = new Cookie();

            if (cookie != null && cookie.Name == "MA")
            {
                // Convert between the System.Net.Cookie to a System.Web.HttpCookie...
                oC.Domain = domain;
                oC.Expires = DateTime.MaxValue;
                oC.Name = cookie.Name;
                oC.Path = cookie.Path;
                oC.Secure = cookie.Secure;
                oC.Value = cookie.Value;
                oC.HttpOnly = cookie.HttpOnly;

                collection.Add(oC);
            }
        }
        return collection;
    }

    public static decimal ToDecimalSafely(object value, decimal defaultValue = 0)
    {
        try
        {
            return Convert.ToDecimal(value);
        }
        catch
        {
            return defaultValue;
        }
    }

    public static int ToIntSafely(object value, int defaultValue = 0)
    {
        try
        {
            return Convert.ToInt32(value);
        }
        catch
        {
            return defaultValue;
        }
    }

    public static string ToJson(object value)
    {
        return JsonConvert.SerializeObject(value);
    }

    #endregion Public Methods

    #region Private Methods

    private static HttpClient InitApiHttpClient(HttpClientHandler handler)
    {
        HttpClient client = new HttpClient(handler);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.AcceptEncoding.Clear();
        client.Timeout = new TimeSpan(0, 0, 1, 0);
        return client;
    }

    #endregion Private Methods
}

public class BatchData
{
    public string Method { get; set; }

    public string RelativeUrl { get; set; }
}

public class NyanyaWebData
{
    #region Public Properties

    public dynamic Data { get; set; }

    public string Message { get; set; }

    public HttpResponseMessage Response { get; set; }

    public bool Result { get; set; }

    public int Status { get; set; }

    #endregion Public Properties
}

// ReSharper restore InconsistentNaming