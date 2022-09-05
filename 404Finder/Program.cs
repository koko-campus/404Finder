using System.Threading;

internal static partial class Program
{
	internal static int Main(string[] args)
	{
		import(); // importing information for env setting.

		if (args.Length == 0)
		{
			Console.WriteLine(" 第一パラメタに検査対象となるURL、または検査対象となるURL一覧を記載したファイルへのパスを指定してください。 ");
			Environment.Exit(1);
		}

		var fstArg = args[0];
		List<Task> tasks = new();

		if (isUrl(fstArg))
        {
			urlWalker(fstArg);
        } else if (File.Exists(fstArg))
        {
			var urls = File.ReadAllLines(fstArg).Where(line => isUrl(line)).ToList().ConvertAll(a => (dynamic)a);
			if (urls.Count == 0)
            {
				Console.WriteLine("指定したファイルに有効なURLがありませんでした。");
				Console.WriteLine("URLは以下の正規表現を満たす必要があります。");
				Console.WriteLine(@"/https?://[\d\w\-\.]+\.\w+/");
				Environment.Exit(1);
			}
			looper(urls, (url, _) => {
				tasks.Add(Task.Run(() => {
					urlWalker(url);
				}));
				return 0;
			});
			Task.WhenAll(tasks).Wait();
        }
		else
		{
			Console.WriteLine("第一パラメタは有効なURLないしはパスではありません。");
			Console.WriteLine("綴りを確認して再度実行してください。");
			Environment.Exit(1);
		}

		return 0;
	}
	internal static readonly string machine = Environment.MachineName;
	internal static readonly string user = Environment.UserName;
}

