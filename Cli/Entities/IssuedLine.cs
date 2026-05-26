using DiffPlex.DiffBuilder.Model;

namespace CSharpierLinter.Entities;

public class IssuedLine
{
    public int LineNumber { get; set; }

    public string Text { get; set; }

    public ChangeType Type { get; set; }
}
