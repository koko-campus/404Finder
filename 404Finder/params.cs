using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

internal static partial class Program
{
	private static Dictionary<string, Dictionary<string, string>> args = new Dictionary<string, Dictionary<string, string>>()
	{
		{"-t", new Dictionary<string, string>() {
			{"value", "10000" },
			{"explanation", "timespan to wait for http response. (millisecond)" },
		} },
		{"-x", new Dictionary<string, string>() {
			{"value", "OFF"},
			{"explanation", "if set, no longer getting asked for confirmation."},
		} },
	};

	private static bool importParams(string[] args)
	{
		for (int i = 1; i < args.Length; i++)
		{
			if (Regex.IsMatch(args[i], @"-(x)"))
			{
				args[i] = "ON";
			}
			else
			{
				args[i] = args[i + 1];
				i++;
			}

		}
		return true;
	}

}

