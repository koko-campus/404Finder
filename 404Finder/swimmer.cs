using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

internal static partial class Program
{
	private static void swimmer(int id, CookieContainer cookie, List<urlStruct> willVisit, List<string> visited, int step)
	{
		List<urlStruct> newComers = new();

		foreach (var visiting in willVisit)
		{
			if (visited.Contains(visiting.url)) continue;
			visited.Add(visiting.url);

			urlExplorer(visiting, cookie);
		}

		swimmer(id, cookie, newComers, visited, step + 1);
	}
}



