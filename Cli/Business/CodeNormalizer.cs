using CSharpierLinter.Interfaces;

namespace CsharpierCodeInsights.Business;

/// <inheritdoc cref="ICodeNormalizer"/>
public class CodeNormalizer : ICodeNormalizer
{
    /// <inheritdoc cref="ICodeNormalizer.NormalizeCode"/>
    public string NormalizeCode(string code)
    {
        return code.Replace("\r\n", "\n");
    }
}
