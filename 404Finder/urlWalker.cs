using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data;

internal static partial class Program
{
    private static void urlWalker(string fqdn)
    {
        // ***** ***** ***** ***** ***** ***** *****
        // ***** ***** execute_log への追加 ***** *****
        // ***** ***** ***** ***** ***** ***** *****

        SQLBuilder SQL = new();
        SQL.Add("SELECT MAX(id) AS updated_id");
        SQL.Add("FROM execute_log;");
        var row = SQL.Select();
        var latestId = row.ContainsKey("updated_id") && row["updated_id"] != DBNull.Value ? int.Parse(row["updated_id"].ToString() ?? "0") + 1 : 0;

        targetIds.Add(latestId);

        SQL.Add("INSERT INTO execute_log(id, fqdn, done_by, done_at)");
        SQL.Add("VALUES(@id, @fqdn, @done_by, @done_at)");
        SQL.SetDataType("@id", SqlDbType.Int);
        SQL.SetDataType("@fqdn", SqlDbType.VarChar);
        SQL.SetDataType("@done_by", SqlDbType.VarChar);
        SQL.SetDataType("@done_at", SqlDbType.VarChar);
        SQL.AddParam(latestId);
        SQL.AddParam(fqdn);
        SQL.AddParam(user);
        SQL.AddParam(machine);
        SQL.Execute();

        CookieContainer cookie = new();
        List<urlStruct> willVisit = new() {
            new urlStruct
			{
                url = fqdn,
                method = httpMethod.get,
                kvp = new Dictionary<string, string>(),
			}
        };
        List<string> visited = new();
        var x = obtain("PAGE_EXPLORERING_DEPTH");
        int max = int.Parse(obtain("PAGE_EXPLORERING_DEPTH"));

        swimmer(latestId, cookie, willVisit, visited, 0, max);

    }
}


