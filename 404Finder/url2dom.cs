using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System.Net;
using System.Text;


internal struct responseStruct
{
	internal string statusCode;
	internal string contentType;
	internal long fileSize;
	internal string charset;
	internal DateTime lastModified;
}

internal static partial class Program
{
	private static (IHtmlDocument, responseStruct) url2dom(urlStruct url, CookieContainer cookie)
	{
		string param = dict2str(url.kvp);
		HttpWebRequest? request = null;
		if (url.method == httpMethod.get)
		{
			param = "?" + param;
			request = GetRequest(url.url, cookie);
		}
		else
		{
			request = PostRequest(url.url, cookie, param);
		}

		try
		{
			var response = (HttpWebResponse)request.GetResponse();
			var contents = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8")).ReadToEnd();
			var dom = new HtmlParser().ParseDocument(contents);
			return (dom, new responseStruct
			{
				statusCode = response.StatusCode.ToString(),
				contentType = response.ContentType,
				fileSize = response.ContentLength,
				charset = response.CharacterSet ?? "",
				lastModified = response.LastModified,
			});
		}
		catch
		{
			return (new HtmlParser().ParseDocument(""), new responseStruct
			{
				statusCode = "404",
				contentType = "",
				fileSize = -1,
				charset = "",
				lastModified = new DateTime(1970, 1, 1),
			});
		}
	}


}


