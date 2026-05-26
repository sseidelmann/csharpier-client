namespace CSharpierLinter.Entities;

public class ScannedFileResult
{
    public ProjectFile ProjectFile { get; set; }

    public List<FormatIssue> IssueList { get; set; } = new List<FormatIssue>();
}
