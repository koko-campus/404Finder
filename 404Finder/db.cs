using System.Data;
using System.Data.SqlClient;

internal enum SQLmethod
{
	Select,
	SelectAll,
	Execute,
}
internal class SQLBuilder
{
	// define member variables here...
	// env data should be "1. sql statement", "2. sql parameters for bind variables", "3. bind variables' data type".
	private string _sql = "";
	private List<dynamic> _sqlParams = new();
	private Dictionary<string, SqlDbType> _sqlParamsDataType = new();
	private SqlConnection connection = new(Program.obtain("DATABASE_CONNECTION_STRING"));

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
		connection.Open();
		SqlTransaction tran = connection.BeginTransaction(IsolationLevel.Serializable);
		try
		{
			// use "use" statement to free the resources easily.
			using (var command = connection.CreateCommand())
			{
				command.Transaction = tran;
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
					List<Dictionary<string, object>> answer = new();
					using (SqlDataReader reader = command.ExecuteReader())
					{
						reset();
						if (sqlmethod == SQLmethod.Select)
						{
							reader.Read();
							Dictionary<string, object> field = new();
							for (var j = 0; j < reader.FieldCount; j++)
							{
								field[reader.GetName(j)] = reader.GetValue(j);
							}
							answer.Add(field);
							return answer;
						}
						else
						{
							while (reader.Read())
							{
								Dictionary<string, object> field = new Dictionary<string, object>();
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
					return new List<Dictionary<string, object>>();
				}
			}

		}
		catch (Exception ex)
		{
			tran.Rollback();
			Console.WriteLine($"ERROR -> {ex.Message}");
			Program.error(ex.Message);
			return new List<Dictionary<string, object>>();
		}
		finally
		{
			if (tran != null) tran.Commit();
			connection.Close();
		}
	}
	internal Dictionary<string, object> Select()
	{
		var nestedAnswer = Run(SQLmethod.Select);
		if (nestedAnswer.Count == 0) return new Dictionary<string, object>();
		return nestedAnswer[0];
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


