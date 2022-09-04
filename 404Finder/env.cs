using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

internal static partial class Program
{
	private static Dictionary<string, string> env = new Dictionary<string, string>();

    private static void import()
    {
        string path = ROOT + ".setting";
        if (!File.Exists(path)) return;
        foreach (var line in File.ReadLines(path))
        {
            if (line.StartsWith("#")) continue;
            if (line.Trim() == "") continue;
            env[Regex.Match(line, @"(?<KEY>[\w\0_]+)=").Groups["KEY"].Value] = Regex.Match(line, @"=(?<VALUE>.*)").Groups["VALUE"].Value.Trim();
        }
    }

    private static string obtain(string key)
	{
        if (!env.ContainsKey(key)) return "";
        return env[key];
	}

}


