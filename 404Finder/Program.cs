using System.Threading;

internal static partial class Program
{
	internal static int Main(string[] args)
	{
		import(); // importing information for env setting.

		var fstArg = args[0];
		List<Task> tasks = new List<Task>();

		if (isUrl(fstArg))
        {
			urlWalker(fstArg);
        } else if (File.Exists(fstArg))
        {
			var urls = File.ReadAllLines(fstArg).Where(line => isUrl(line)).ToList().ConvertAll(a => (dynamic)a);
			if (urls.Count() == 0)
            {
				Console.WriteLine(" file you gave has no valid url inside. ");
				Console.WriteLine(" show urls.txt to get infomation. ");
				helper();
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
			Console.WriteLine(" you sent invalid param(s). ");
			Console.WriteLine(" show parameters explanation below. ");
			helper();
			Environment.Exit(1);
        }

		return 0;
	}

}

