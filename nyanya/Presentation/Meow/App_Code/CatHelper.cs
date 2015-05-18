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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HttpCookie = System.Web.HttpCookie;

// ReSharper disable InconsistentNaming

public static class CatHelper
{
    public static string MD5(object md5Key){
        byte[] result = Encoding.Default.GetBytes(md5Key.ToString());
        MD5 md5 = new MD5CryptoServiceProvider();  
        byte[] output = md5.ComputeHash(result);  
        return BitConverter.ToString(output).Replace("-","");
    }
    
    #region Public Methods

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

    public static string GetLoginUrl(string url = "")
    {
        string baseUrl = "~/passport/login";
        if (!string.IsNullOrEmpty(url))
        {
            baseUrl = baseUrl + "?backUrl=" + HttpUtility.UrlEncode(url);
        }
        return baseUrl;
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

    public static decimal SyPrice(decimal price, decimal yield, decimal period, decimal year = 360.00m)
    {
        return Math.Floor(price * yield * period / year) / 100;
    }

    public static decimal SyPrice(object price, object yield, object period)
    {
        decimal d = SyPrice(Convert.ToDecimal(price), Convert.ToDecimal(yield), Convert.ToDecimal(period));
        return decimal.Round(d, 2, MidpointRounding.AwayFromZero);
    }

    public static decimal ToDecimalSafely(this object value, decimal defaultValue = 0)
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

    public static int ToIntSafely(this object value, int defaultValue = 0)
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

    public static string ToJson(this object value)
    {
        return JsonConvert.SerializeObject(value);
    }

    #endregion Public Methods
}