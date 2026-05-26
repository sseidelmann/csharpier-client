using System.Text;

namespace CSharpierLinter.Entities;

public class FormatIssue
{
    public int OriginalLineNumber { get; set; }
    public StringBuilder DiffBuilder { get; set; }

    public string DiffText => DiffBuilder.ToString();

    public ProjectFile ProjectFile { get; set; }

    public List<IssuedLine> IssuedLines { get; set; }

    public FormatIssue(int originalLineNumber, ProjectFile projectFile)
    {
        OriginalLineNumber = originalLineNumber;
        ProjectFile = projectFile;
        DiffBuilder = new StringBuilder();
        IssuedLines = new List<IssuedLine>();
    }

    public AffectedLine AffectedLine
    {
        get
        {
            var ordered = IssuedLines.OrderBy(x => x.LineNumber);
            return new AffectedLine()
            {
                Lower = ordered.First().LineNumber,
                Upper = ordered.Last().LineNumber,
            };
        }
    }
}
