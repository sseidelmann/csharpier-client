namespace CsharpierCodeInsights.Cli;

public class CliHandler
{
    private readonly string[] _args;

    public CliHandler(string[] args)
    {
        _args = args;
    }

    public bool ShouldShowHelp()
    {
        return _args.Contains("--help") || _args.Contains("-h");
    }

    public void ShowHelp()
    {
        Console.WriteLine("Usage: CsharpierCodeInsights [options] [directory]");
        Console.WriteLine();
        Console.WriteLine("Arguments:");
        Console.WriteLine("  directory             The directory to scan. Defaults to the current directory.");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  --output <target>     The output target. Can be 'console' or 'bitbucket'. Defaults to 'console'.");
        Console.WriteLine("  --help                Show this help message.");
    }

    public string GetOutputTarget()
    {
        for (var i = 0; i < _args.Length; i++)
        {
            if (_args[i] != "--output" || i + 1 >= _args.Length) continue;
            var outputTarget = _args[i + 1].ToLower();
            if (outputTarget is "console" or "bitbucket")
            {
                return outputTarget;
            }
        }

        return "console";
    }

    public string GetWorkingDirectory()
    {
        return _args.FirstOrDefault(arg => !arg.StartsWith("-")) ?? Directory.GetCurrentDirectory();
    }
}
