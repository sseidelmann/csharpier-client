using CsharpierCodeInsights.Business;
using CsharpierCodeInsights.Cli;
using CSharpierLinter.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CsharpierCodeInsights.Factories;

public class OutputWriterFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly CliHandler _cliHandler;

    public OutputWriterFactory(IServiceProvider serviceProvider, CliHandler cliHandler)
    {
        _serviceProvider = serviceProvider;
        _cliHandler = cliHandler;
    }

    public IOutputWriter Create()
    {
        var outputTarget = _cliHandler.GetOutputTarget();
        return outputTarget switch
        {
            "console" => _serviceProvider.GetRequiredService<ConsoleOutputWriter>(),
            "bitbucket" => _serviceProvider.GetRequiredService<BitbucketOutputWriter>(),
            _ => throw new ArgumentException($"Invalid output target: {outputTarget}", nameof(outputTarget))
        };
    }
}
