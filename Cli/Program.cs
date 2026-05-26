using System.Text.Json.Serialization;
using CsharpierCodeInsights.Business;
using CsharpierCodeInsights.Cli;
using CsharpierCodeInsights.Factories;
using CSharpierLinter.Business;
using CSharpierLinter.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CsharpierCodeInsights;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddTransient<CliHandler>(_ => new CliHandler(args));

        serviceCollection.AddTransient<IIssueExtractor, IssueExtractor>();
        serviceCollection.AddTransient<ICodeNormalizer, CodeNormalizer>();
        serviceCollection.AddTransient<ICSharpierServerClient, CSharpierServerClient>();
        serviceCollection.AddTransient<IFileLocator, FileLocator>();
        serviceCollection.AddTransient<ICSharpierScanner, CSharpierScanner>();
        
        // writer
        serviceCollection.AddTransient<ConsoleOutputWriter>();
        serviceCollection.AddTransient<BitbucketOutputWriter>();
        serviceCollection.AddSingleton<OutputWriterFactory>();
        serviceCollection.AddTransient<IOutputWriter>(sp => sp
            .GetRequiredService<OutputWriterFactory>()
            .Create());

        // http
        serviceCollection.AddHttpClient();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var cliHandler = serviceProvider.GetRequiredService<CliHandler>();
        if (cliHandler.ShouldShowHelp())
        {
            cliHandler.ShowHelp();
            return 0;
        }

        var scanner = serviceProvider.GetRequiredService<ICSharpierScanner>();
        var outputWriter = serviceProvider.GetRequiredService<IOutputWriter>();

        try
        {
            var scannerResult = await scanner.ScanAsync();

            await outputWriter.Write(scannerResult);

            return scannerResult.TotalIssues > 0 ? 1 : 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return 1;
    }
}