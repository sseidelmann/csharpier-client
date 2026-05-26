using Microsoft.CodeAnalysis;

namespace CSharpierLinter.Entities;

public class FormatResponseError
{
    public required FileLinePositionSpan LineSpan { get; set; }
    public required string Description { get; set; }
}