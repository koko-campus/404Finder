using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static partial class Program
{
	private static List<string> _log = new();
	private static List<string> _error = new();
	internal static void log(string message)
	{
		_log.Add(message);
	}

	internal static void error(string message)
	{
		_error.Add(message);
		errorCount++;
	}

	private static void writeLog()
	{
		File.WriteAllLines(ROOT + ".log", _log);
		File.WriteAllLines(ROOT + ".error", _error);
	}
}

