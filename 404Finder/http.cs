using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

internal static partial class Program
{
    internal static HttpWebRequest GetRequest(string url, CookieContainer cookie)
    {
        var request = GetRequestBase(url, cookie);
        request.Timeout = int.Parse(obtain("REQUEST_TIMEOUT_SPAN"));
        return request;
    }
    private static HttpWebRequest GetRequestBase(string url, CookieContainer cookie)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));

        request.AllowAutoRedirect = true;

        request.UserAgent = obtain("USER_AGENT");
        request.Method = "GET";
        request.CachePolicy = new System.Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore);
        if (cookie != null) request.CookieContainer = cookie;
        return request;
    }
    internal static HttpWebRequest PostRequest(string url, CookieContainer cookie, string body)
    {
        var enc = Encoding.GetEncoding(obtain("CHARSET"));
        return PostRequest(url, cookie, body, enc);
    }
    internal static HttpWebRequest PostRequest(string url, CookieContainer cookie, Dictionary<string, string> dict)
    {
        return PostRequest(url, cookie, dict2str(dict));
    }

    internal static HttpWebRequest PostRequest(string url, CookieContainer cookie, string body, Encoding encoding)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));

        request.AllowAutoRedirect = true;

        request.MaximumAutomaticRedirections = int.Parse(obtain("REDIRECT_LIMIT"));
        request.UserAgent = obtain("USER_AGENT");
        request.Method = "POST";
        request.CachePolicy = new System.Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore);
        request.Timeout = int.Parse(obtain("REQUEST_TIMEOUT_SPAN"));
        if (cookie != null) request.CookieContainer = cookie;
        var b = encoding.GetBytes(body);
        request.ContentLength = b.Length;
        request.ContentType = "application/x-www-form-urlencoded";
        using (var stream = request.GetRequestStream())
        {
            stream.Write(b, 0, b.Length);
        }
        return request;
    }
}
