using System.Text;
using CSharpierLinter.Entities;
using CSharpierLinter.Interfaces;
using DiffPlex.DiffBuilder.Model;

namespace CsharpierCodeInsights.Business;

public class ConsoleOutputWriter : IOutputWriter
{
    public Task Write(ScannerResult scannerResult)
    {
        Console.WriteLine("Scan-Ergebnis:");
        foreach (var annotation in scannerResult.ScannedFileResults)
        {
            foreach (var issue in annotation.IssueList)
            {
                var message = new StringBuilder();

                foreach (var line in issue.IssuedLines)
                {
                    switch (line.Type)
                    {
                        case ChangeType.Deleted:
                            // message.Append($"{line.LineNumber,4}  - {line.Text}\n");
                            message.Append($"- {line.Text}\n");
                            break;
                        case ChangeType.Inserted:
                            // message.Append($"{line.LineNumber,4}  + {line.Text}\n");
                            message.Append($"+ {line.Text}\n");
                            break;
                        case ChangeType.Modified:
                            // message.Append($"{line.LineNumber,4}  o {line.Text}\n");
                            message.Append($"o {line.Text}\n");
                            break;
                    }
                }
                Console.WriteLine($"{issue.ProjectFile.RelativePath}:{issue.OriginalLineNumber}");
                Console.WriteLine($"{message}");
            }
        }
        return Task.CompletedTask;
    }
}
