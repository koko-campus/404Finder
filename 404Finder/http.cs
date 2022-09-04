using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace scriptCalledFromCreditcardFormIntegrityChecker
{
    class HttpClient
    {
        internal static HttpWebRequest GetRequestForSpeedCheck(string url)
        {
            var request = GetRequestBase(url, null);
            request.Timeout = (int)Properties.Settings.Default["Request_Timeout_SpeedCheck"];
            request.Headers.Add("Cache-Control", "no-store, max-age=0");
            return request;
        }
        internal static HttpWebRequest GetRequest(string url, CookieContainer cookie)
        {
#if DEBUG
            Console.WriteLine("■■■ GET: " + url);
#endif
            var request = GetRequestBase(url, cookie);
            request.Timeout = 30000; // (int)Properties.Settings.Default["Request_Timeout"];
            return request;
        }
        private static HttpWebRequest GetRequestBase(string url, CookieContainer cookie)
        {
            var uriobj = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriobj);
            //リダイレクトを追従する
            request.AllowAutoRedirect = true;
            //最大リダイレクト回数 (未設定時:50)
            //request.MaximumAutomaticRedirections = 10;
            var cache = new System.Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore);
            request.UserAgent = Properties.Settings.Default["Request_UserAgent"].ToString();
            request.Method = "GET";
            request.CachePolicy = cache;
            if (cookie != null) request.CookieContainer = cookie;
            return request;
        }
        internal static HttpWebRequest PostRequest(string url, CookieContainer cookie, string body)
        {
            var enc = Encoding.GetEncoding(Properties.Settings.Default["Request_Encoding"].ToString());
            return PostRequest(url, cookie, body, enc);
        }
        internal static HttpWebRequest PostRequest(string url, CookieContainer cookie, Dictionary<string, string> dict)
        {
            return PostRequest(url, cookie, dict2str(dict));
        }
        internal static string dict2str(Dictionary<string, string> dict)
        {
            return string.Join("&", dict.Select(pair => $"{pair.Key}={pair.Value}"));
        }
        internal static HttpWebRequest PostRequest(string url, CookieContainer cookie, string body, Encoding encoding)
        {
#if DEBUG
            Console.WriteLine("■■■ POST: " + url + " | " + body);
#endif
            var uriobj = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriobj);
            //リダイレクトを追従する
            request.AllowAutoRedirect = true;
            //最大リダイレクト回数
            request.MaximumAutomaticRedirections = 10;
            var cache = new System.Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore);
            request.UserAgent = Properties.Settings.Default["Request_UserAgent"].ToString();
            request.Method = "POST";
            request.CachePolicy = cache;
            request.Timeout = 10000; // (int)Properties.Settings.Default["Request_Timeout"];
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
}