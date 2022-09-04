using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


internal partial class Program
{
	private static dynamic rec(Func<dynamic, Func<dynamic, dynamic>> fx) => fx(fx);
	private static dynamic rec(Func<dynamic, Func<dynamic, dynamic, dynamic>> fx) => fx(fx);
	private static dynamic rec(Func<dynamic, Func<dynamic, dynamic, dynamic, dynamic>> fx) => fx(fx);

	private static List<dynamic> looper(List<dynamic> list, Func<dynamic, int, dynamic> fx, int i = 0) => (i < list.Count())
			? new List<dynamic>() { { fx(list[i], i) } }.Concat(looper(list, fx, i + 1)).ToList() : new List<dynamic>();

	private static List<dynamic> map(Func<dynamic, int, dynamic> fx, List<dynamic> list, int i = 0) => (i < list.Count())
			? new List<dynamic>() { { fx(list[i], i) } }.Concat(map(fx, list, i + 1)).ToList() : new List<dynamic>();

	private static List<dynamic> filter(Func<dynamic, bool> fx, List<dynamic> list, int i = 0) => (i < list.Count() && fx(list[i]))
			? new List<dynamic>() { { list[i] } }.Concat(filter(fx, list, i + 1)).ToList() : (i < list.Count())
			? new List<dynamic>() { }.Concat(filter(fx, list, i + 1)).ToList() : new List<dynamic>() { };

	private static bool all(List<dynamic> list, Func<dynamic, bool> fx, int i = 0) => (i < list.Count())
			? fx(list[i]) && all(list, fx, i + 1) : true;

	private static bool any(List<dynamic> list, Func<dynamic, bool> fx, int i = 0) => (i < list.Count())
			? fx(list[i]) || any(list, fx, i + 1) : false;

	private static dynamic? findOne(List<dynamic> list, Func<dynamic, bool> fx, int i = 0) => (i < list.Count() && fx(list[i]))
			? list[i] : (i < list.Count())
			? findOne(list, fx, i + 1) : null;

	private static string? coalesce(params string[] args) => (args.Count() != 0)
			? isTruthy(args[0])
			? args[0] : coalesce(args.ToList().GetRange(1, args.Count() - 1).ToArray()) : null;

	private static bool isTruthy(string a) => a != null && a != "";

	private static string mergeUrl(string url, string path) => isUrl(path)
			? path : path.StartsWith("/")
			? takeFqdn(url) + path : url.EndsWith("/")
			? url + path : url + "/" + path;

	private static bool isUrl(string url) => Regex.IsMatch(url, @"https?://[\d\w\-\.]+");
	private static string takeFqdn(string url) => Regex.Match(url, @"(?<FQDN_ALPHA>https?://[\d\w\-\.]+)").Groups["FQDN_ALPHA"].Value;

	private static string takeDomain(string url) => Regex.Match(url, @"https?://(?<DOMAIN>[\d\w\-\.]+)").Groups["DOMAIN"].Value;

	private static List<dynamic> cuttoff(List<dynamic> list, int i) => (list.Count() == 0)
		? new List<dynamic>() : (list.Count() - 1 < i)
		? list.GetRange(0, list.Count() - 1) : list.GetRange(0, i);

	private static List<dynamic> cuttoff(List<dynamic> list, int i, int j) => (list.Count() == 0)
			? new List<dynamic>() : (list.Count() - 1 < j)
			? list.GetRange(i, list.Count() - 1) : list.GetRange(i, j);

	private static string dict2str(Dictionary<string, string> dict) => string.Join("&", dict.Select(pair => $"{pair.Key}={pair.Value}"));

	private static string date() => DateTime.Now.ToString("yyyy_MM_dd");
	private static string datetime() => DateTime.Now.ToString("yyyy_MM_dd__HH_mm_ss");

	private static bool isValidUrl(string url) => Regex.IsMatch(url, @"https?://[\d\w_\-\.]+");
}
