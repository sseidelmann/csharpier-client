using Cgnd.CSharpier.Cli.Entities;
using CsharpierCodeInsights;
using CSharpierLinter.Entities;

namespace CSharpierLinter.Interfaces;

public interface ICSharpierServerClient
{
    public Task<FormatResponse?> FormatAsync(FormatRequest request);
}
