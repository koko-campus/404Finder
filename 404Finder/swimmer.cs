using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

internal static partial class Program
{
	private static void swimmer(int id, CookieContainer cookie, List<urlStruct> willVisit, List<string> visited, counter counter, ref exploringSetting setting)
	{
		if (setting.limit < counter.step) return;
		List<urlStruct> newComers = new();

		foreach (var visiting in willVisit)
		{
			// 既に探索済みのURLを判定!!!
			Console.WriteLine($" url => {visiting.url} | step({counter.step}) | count({counter.count}) ");
			if (visited.Contains(visiting.url)) continue;
			visited.Add(visiting.url);
			counter.count++;
			newComers = newComers.Concat(urlExplorer(visiting, id, counter, setting, cookie)).ToList();
		}

		counter.step++;

		swimmer(id, cookie, newComers, visited, counter, ref setting);
	}
}



