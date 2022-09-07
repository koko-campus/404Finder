using System;
using System.Collections.Generic;
using System.Net;
using System.Data;
using AngleSharp.Html.Dom;
using System.Text.RegularExpressions;

internal struct resultStruct
{
	internal int id;
	internal string path;
	internal string ext;
	internal string status_code;
	internal string content_type;
	internal DateTime last_modified;
	internal long file_size;
	internal string charset;
	internal TimeSpan responseTime;
	internal int step;
	internal string comeFrom;
}

internal static partial class Program
{
	private static List<resultStruct> results = new();

	private static List<urlStruct> urlExplorer(urlStruct url, int id, counter counter, exploringSetting setting, CookieContainer cookie)
	{
		if (setting.limit < counter.count) return new List<urlStruct>();
		if (!isUrl(url.url)) return new List<urlStruct>();

		(var dom, var responseData) = url2dom(url, cookie);
		
		// 探索対象となる(リンクを有する)タグを取得

		var a = dom.GetElementsByTagName("a").Where(aDoc => aDoc.GetAttribute("href") != null).Select(link => link.GetAttribute("href")).ToList();
		var img = dom.GetElementsByTagName("img").Where(imgDoc => imgDoc.GetAttribute("src") != null).Select(link => link.GetAttribute("src")).ToList();
		var js = dom.GetElementsByTagName("script").Where(jsDoc => jsDoc.GetAttribute("src") != null).Select(link => link.GetAttribute("src")).ToList();
		var css = dom.GetElementsByTagName("link").Where(link => link.GetAttribute("rel") != null && link.GetAttribute("rel") == "stylesheet" && link.GetAttribute("href") != null).Select(link => link.GetAttribute("href")).ToList();

		var links = new []{
			a, img, js, css
		}.SelectMany(_ => _).Where(doc => doc != null).Select(link => new urlStruct
		{
			url = mergeUrl(url.url, link ?? ""),
			comeFrom = url.url,
			method = httpMethod.get,
			kvp = new Dictionary<string, string>(),
		}).ToList();

		registerToDB(new resultStruct {
			id = id,
			path = new Uri(url.url).LocalPath,
			ext = Path.GetExtension(url.url),
			status_code = responseData.statusCode,
			content_type = responseData.contentType,
			last_modified = responseData.lastModified,
			file_size = responseData.fileSize,
			charset = responseData.charset,
			responseTime = responseData.responseTime,
			step = counter.step,
			comeFrom = url.comeFrom ?? "",
		});

		return links;
	}

	private static void registerToDB(resultStruct result)
	{
		SQLBuilder SQL = new();

		SQL.Add("INSERT INTO result(id, path, ext, status_code, content_type, last_modified, file_size, charset, step, come_from, response_time)");
		SQL.Add("VALUES(@id, @path, @ext, @status_code, @content_type, @last_modified, @file_size, @charset, @step, @come_from, @response_time);");

		SQL.SetDataType("@id", SqlDbType.Int);
		SQL.SetDataType("@path", SqlDbType.VarChar);
		SQL.SetDataType("@ext", SqlDbType.VarChar);
		SQL.SetDataType("@status_code", SqlDbType.VarChar);
		SQL.SetDataType("@content_type", SqlDbType.VarChar);
		SQL.SetDataType("@last_modified", SqlDbType.DateTime);
		SQL.SetDataType("@file_size", SqlDbType.Int);
		SQL.SetDataType("@charset", SqlDbType.VarChar);
		SQL.SetDataType("@step", SqlDbType.Int);
		SQL.SetDataType("@come_from", SqlDbType.VarChar);
		SQL.SetDataType("@response_time", SqlDbType.Int);

		SQL.AddParam(result.id);
		SQL.AddParam(result.path);
		SQL.AddParam(result.ext);
		SQL.AddParam(result.status_code);
		SQL.AddParam(result.content_type);
		SQL.AddParam(result.last_modified);
		SQL.AddParam(result.file_size);
		SQL.AddParam(result.charset);
		SQL.AddParam(result.step);
		SQL.AddParam(result.comeFrom);
		SQL.AddParam((int)result.responseTime.TotalMilliseconds);

		SQL.Execute();
	}

}

