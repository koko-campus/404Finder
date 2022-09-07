using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

internal static partial class Program
{
	private static void swimmer(int id, CookieContainer cookie, List<urlStruct> willVisit, List<string> visited, int step, int max)
	{
		if (max < step) return;
		List<urlStruct> newComers = new();

		foreach (var visiting in willVisit)
		{
			Console.WriteLine($" url => {visiting.url} | step({step}) ");
			if (visited.Contains(visiting.url)) continue;
			visited.Add(visiting.url);

			newComers = newComers.Concat(urlExplorer(visiting, id, step, cookie)).ToList();
		}

		swimmer(id, cookie, newComers, visited, step + 1, max);
	}
}



