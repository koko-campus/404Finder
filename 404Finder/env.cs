using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static partial class Program
{
	private static Dictionary<string, List<string>> env = new Dictionary<string, List<string>>();

    private static void import()
    {
        state currentState = state.undefined;
        string path = Env.Get("root") + ".whitelist";
        if (!File.Exists(path)) return;
        foreach (var line in File.ReadLines(path))
        {
            string pattern = "";
            for (int i = 0; i < line.Length; i++)
            {
                char character = line[i];
                if (i == 0 && character == '#') break; // comment out
                if (i == 0 && character == '~') // state switcher
                {
                    if (Regex.IsMatch(line, @"~LINE~")) currentState = state.ofLine;
                    if (Regex.IsMatch(line, @"~WORD~")) currentState = state.ofWord;
                    break;
                }
                pattern += character;
            }
            pattern = pattern.Trim();
            if (pattern == "") continue;
            if (currentState == state.ofLine) patternsOnLines.Add(pattern);
            if (currentState == state.ofWord) patternsOnWords.Add(pattern);
        }
    }
    private static bool IsSafeOnLine(string line)
    {
        foreach (var pattern in patternsOnLines)
        {
            if (Regex.IsMatch(line, pattern))
            {
                log.ignored($"LINE\t{line.PadRight(30)}\tskipped by whitelist (of line) [pattern -> {pattern}]");
                return true;
            }
        }
        return false;
    }
    private static bool IsSafeOnWord(string word)
    {
        foreach (var pattern in patternsOnWords)
        {
            if (word == pattern)
            {
                log.ignored($"WORD\t{word.PadRight(30)}\tskipped by whitelist (of word) [pattern -> {pattern}]");
                return true;
            }
        }
        return false;
    }

}


