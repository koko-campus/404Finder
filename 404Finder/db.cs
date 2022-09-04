using System.Data;
using System.Data.SqlClient;

internal enum SQLmethod
{
    Select,
    SelectAll,
    Execute,
}
internal enum whichDB
{
    connectionStringProbi,
    connectionStringIChecker,
}
internal class SQLBuilder
{
    private whichDB which;
    internal SQLBuilder(whichDB either)
    {
        which = either;
    }
    // define member variables here...
    // env data should be "1. sql statement", "2. sql parameters for bind variables", "3. bind variables' data type".
    private string _sql = "";
    private List<dynamic> _sqlParams = new List<dynamic> { };
    private Dictionary<string, SqlDbType> _sqlParamsDataType = new Dictionary<string, SqlDbType> { };
    // add sql statement.
    internal void Add(string sql)
    {
        // put spaces surrounding received sql statement and concat one with accumulated sql.
        _sql += $" {sql} ";
    }
    // add sql parameter.
    internal void AddParam(dynamic param)
    {
        _sqlParams.Add(param);
    }
    // set parametes' data type.
    internal void SetDataType(string name, SqlDbType sqlDbType)
    {
        _sqlParamsDataType[name] = sqlDbType;
    }
    // reset SQLBuilder contents.
    // including (sql statement, sql parameters, sql statements for transaction, sql parameters for transaction).
    void reset()
    {
        _sql = "";
        _sqlParams.Clear();
        _sqlParamsDataType.Clear();
    }
    // execute "SELECT" sql statement
    // arg -> nothing
    // return -> List<List<object>>
    private List<Dictionary<string, object>> Run(SQLmethod sqlmethod)
    {
        string SQLExpress = Program.obtain("DATABASE_CONNECTION_STRING");
        using (var connection = new SqlConnection(SQLExpress))
        {
            connection.Open();
            // use "use" statement to free the resources easily.
            using (var command = connection.CreateCommand())
            {
                // put sql statement on CommandText.
                command.CommandText = _sql;
                // set sqlParamsType on CommandParameters.
                foreach (var paramType in _sqlParamsDataType)
                {
                    command.Parameters.Add(new SqlParameter(paramType.Key, paramType.Value));
                }
                // put sqlParams on CommandParameters.
                for (int i = 0; i < _sqlParams.Count; i++)
                {
                    command.Parameters[i].Value = _sqlParams[i];
                }
                if (sqlmethod != SQLmethod.Execute)
                {
                    // when it's called from Select fx or SelectAll fx, return nested List.
                    // regardless of single result or multiple result.
                    List<Dictionary<string, object>> answer = new List<Dictionary<string, object>> { };
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reset();
                        if (sqlmethod == SQLmethod.Select)
                        {
                            reader.Read();
                            Dictionary<string, object> field = new Dictionary<string, object> { };
                            for (var j = 0; j < reader.FieldCount; j++)
                            {
                                field[reader.GetName(j)] = reader.GetValue(j);
                            }
                            // suppose receiving JUST ONE record.
                            // if records are multiple, run error.
                            if (reader.Read())
                            {
                                throw new Exception("matched records are multiple. use 'SelectAll' fx instead");
                            }
                            answer.Add(field);
                            return answer;
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                Dictionary<string, object> field = new Dictionary<string, object> { };
                                for (var j = 0; j < reader.FieldCount; j++)
                                {
                                    field[reader.GetName(j)] = reader.GetValue(j);
                                }
                                answer.Add(field);
                            }
                        }
                    }
                    return answer;
                }
                else
                {
                    //Console.WriteLine(_sql);
                    command.ExecuteNonQuery();
                    reset();
                    // when it's called from Execute fx, return empty List nested.
                    return new List<Dictionary<string, object>> { };
                }
            }
        }
    }
    internal Dictionary<string, object> Select()
    {
        return Run(SQLmethod.Select)[0];
    }
    internal List<Dictionary<string, object>> SelectAll()
    {
        return Run(SQLmethod.SelectAll);
    }
    internal void Execute()
    {
        Run(SQLmethod.Execute);
        return;
    }
}
