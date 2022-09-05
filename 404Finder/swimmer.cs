using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static partial class Program
{
	private static void swimmer(int id, List<string> willVisit, List<string> visited, int step)
	{
		List<string> newComers = new();

		foreach (var visiting in willVisit)
		{
			if (visited.Contains(visiting)) continue;
			visited.Add(visiting);

			urlExplorer();
		}

		swimmer(id, newComers, visited, step + 1);
	}
}



