using CSharpierLinter.Entities;

namespace CSharpierLinter.Interfaces;

/// <summary>
/// Interface for extracting issues between old and new texts
/// </summary>
public interface IIssueExtractor
{
    /// <summary>
    /// Extracts the issues from the formatted
    /// </summary>
    /// <param name="projectFile">The project file</param>
    /// <param name="originalText">The original text</param>
    /// <param name="formattedText">The formatted text</param>
    /// <returns></returns>
    public List<FormatIssue> ExtractIssues(
        ProjectFile projectFile,
        string originalText,
        string formattedText
    );
}
