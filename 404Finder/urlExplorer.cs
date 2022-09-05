using System;
using System.Collections.Generic;
using System.Net;
using AngleSharp.Html.Dom;

internal static partial class Program
{
	private static List<urlStruct> urlExplorer(urlStruct url, CookieContainer cookie)
	{
		var dom = url2dom(url, cookie);
		
		// 探索対象となる(リンクを有する)タグを取得

		var a = dom.GetElementsByTagName("a").Where(aDoc => aDoc.GetAttribute("href") != null).Select(link => link.GetAttribute("href")).ToList();
		var img = dom.GetElementsByTagName("img").Where(imgDoc => imgDoc.GetAttribute("src") != null).Select(link => link.GetAttribute("src")).ToList();
		var js = dom.GetElementsByTagName("script").Where(jsDoc => jsDoc.GetAttribute("src") != null).Select(link => link.GetAttribute("src")).ToList();
		var css = dom.GetElementsByTagName("link").Where(link => link.GetAttribute("rel") != null && link.GetAttribute("rel") == "stylesheet" && link.GetAttribute("href") != null).Select(link => link.GetAttribute("href")).ToList();

		var links = new []{
			a, img, js, css
		}.SelectMany(_ => _).Where(doc => doc != null).Select(link => new urlStruct
		{
			url = mergeUrl(url.url, link),
			method = httpMethod.get,
		});


	}
}

