using System;
using System.Collections.Generic;
using System.Text;
using CSharpierLinter.Entities;
using CSharpierLinter.Interfaces;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

namespace CSharpierLinter.Business;

/// <inheritdoc cref="IIssueExtractor"/>
public class IssueExtractor : IIssueExtractor
{
    /// <inheritdoc cref="IIssueExtractor.ExtractIssues"/>
    public List<FormatIssue> ExtractIssues(
        ProjectFile projectFile,
        string originalText,
        string formattedText
    )
    {
        var diff = InlineDiffBuilder.Instance.BuildDiffModel(originalText, formattedText);
        var issues = new List<FormatIssue>();

        int currentOriginalLine = 0;
        FormatIssue? activeIssue = null;

        // Wir merken uns die Zeilennummer des letzten geänderten Blocks,
        // um zu sehen, ob wir uns noch im selben Kontext befinden.
        int lastChangedOriginalLine = -1;

        foreach (var line in diff.Lines)
        {
            // Zeilennummer der Originaldatei mitzählen
            if (line.Type != ChangeType.Inserted)
            {
                currentOriginalLine++;
            }

            if (line.Type == ChangeType.Inserted || line.Type == ChangeType.Deleted)
            {
                int targetLine =
                    line.Type == ChangeType.Inserted
                        ? Math.Max(1, currentOriginalLine)
                        : currentOriginalLine;

                if (
                    activeIssue != null
                    && lastChangedOriginalLine != -1
                    && (targetLine - lastChangedOriginalLine) > 1
                )
                {
                    issues.Add(activeIssue);
                    activeIssue = null;
                }

                if (activeIssue == null)
                {
                    activeIssue = new FormatIssue(targetLine, projectFile);
                }

                string prefix = line.Type == ChangeType.Inserted ? "+ " : "- ";
                activeIssue.DiffBuilder.AppendLine($"[{currentOriginalLine}]  {prefix}{line.Text}");
                activeIssue.IssuedLines.Add(
                    new IssuedLine()
                    {
                        LineNumber = targetLine,
                        Text = line.Text,
                        Type = line.Type,
                    }
                );

                lastChangedOriginalLine = currentOriginalLine;
            }
            else
            {
                if (activeIssue != null && (currentOriginalLine - lastChangedOriginalLine) > 1)
                {
                    issues.Add(activeIssue);
                    activeIssue = null;
                }
            }
        }

        if (activeIssue != null)
        {
            issues.Add(activeIssue);
        }

        return issues;
    }
}
