using System;
using System.Linq;
using CSharpierLinter.Business;
using CSharpierLinter.Entities;
using NUnit.Framework;

namespace CSharpierLinter.Tests;

[TestFixture]
public class IssueExtractorTests
{
    private IssueExtractor _sut;

    [SetUp]
    public void SetUp()
    {
        // System Under Test initialisieren
        _sut = new IssueExtractor();
    }

    [Test]
    public void ExtractIssues_WhenTextsAreIdentical_ShouldReturnEmptyList()
    {
        // Arrange
        var oldText = "public class Test {}\npublic int Id { get; set; }";
        var newText = "public class Test {}\npublic int Id { get; set; }";
        var projectFile = new ProjectFile() { FullPath = "/tmp/File.cs", RelativePath = "File.cs" };

        // Act
        var result = _sut.ExtractIssues(projectFile, oldText, newText);

        // Assert
        Assert.That(
            result,
            Is.Empty,
            "Bei identischem Code sollten keine Issues extrahiert werden."
        );
    }

    [Test]
    public void ExtractIssues_WhenSingleBlockIsChanged_ShouldReturnOneIssueWithCorrectLineNumber()
    {
        // Arrange
        var oldText = "using System;\n\npublic class Test {\n    public int Id {get;set;}\n}";
        var newText = "using System;\n\npublic class Test {\n    public int Id { get; set; }\n}";
        var projectFile = new ProjectFile() { FullPath = "/tmp/File.cs", RelativePath = "File.cs" };

        // Act
        var result = _sut.ExtractIssues(projectFile, oldText, newText);

        // Assert
        Assert.That(result, Has.Count.EqualTo(1));
        var issue = result.First();
        Assert.That(
            issue.OriginalLineNumber,
            Is.EqualTo(4),
            "Die Zeilennummer des Issues sollte exakt auf die fehlerhafte Zeile im Original zeigen."
        );
        Assert.That(issue.DiffText, Contains.Substring("-     public int Id {get;set;}"));
        Assert.That(issue.DiffText, Contains.Substring("+     public int Id { get; set; }"));
    }

    [Test]
    public void ExtractIssues_WhenTwoConsecutiveIssuesExistWithoutUnchangedLines_ShouldSplitIntoTwoIssues()
    {
        // Arrange - Dein Fallback-Szenario aus der Praxis
        var oldText =
            "application.MapFallbackToFile(\n    \"/{culture}/users/{**path}\",\n    \"/index.html\"\n);\napplication.MapFallbackToFile(\n    \"/index.html\"\n);";
        var newText =
            "application.MapFallbackToFile(\"/{culture}/users/{**path}\", \"/index.html\");\napplication.MapFallbackToFile(\"/index.html\");";
        var projectFile = new ProjectFile() { FullPath = "/tmp/File.cs", RelativePath = "File.cs" };

        // Act
        var result = _sut.ExtractIssues(projectFile, oldText, newText);

        // Assert
        Assert.That(
            result,
            Has.Count.EqualTo(2),
            "Die aufeinanderfolgenden Änderungen müssen als zwei separate Issues erkannt werden."
        );

        var firstIssue = result[0];
        var secondIssue = result[1];

        Assert.That(
            firstIssue.OriginalLineNumber,
            Is.EqualTo(1),
            "Das erste Issue beginnt bei Zeile 1."
        );
        Assert.That(
            secondIssue.OriginalLineNumber,
            Is.EqualTo(5),
            "Das zweite Issue beginnt bei Zeile 5."
        );
    }

    [Test]
    public void ExtractIssues_WhenPureInsertionAtTheEnd_ShouldReturnIssuePointingToLastLine()
    {
        // Arrange
        var oldText = "public class Test\n{\n}";
        var newText = "public class Test\n{\n}\n// Ein neuer Kommentar am Ende";
        var projectFile = new ProjectFile() { FullPath = "/tmp/File.cs", RelativePath = "File.cs" };

        // Act
        var result = _sut.ExtractIssues(projectFile, oldText, newText);

        // Assert
        Assert.That(result, Has.Count.EqualTo(1));
        var issue = result.First();
        Assert.That(
            issue.OriginalLineNumber,
            Is.EqualTo(3),
            "Reine Einfügungen am Ende sollten auf die letzte bekannte Originalzeile verweisen."
        );
        Assert.That(issue.DiffText, Contains.Substring("+ // Ein neuer Kommentar am Ende"));
    }

    [Test]
    public void ExtractIssues_WhenPureDeletionAtTheStart_ShouldReturnIssueWithLineOne()
    {
        // Arrange
        var oldText = "// Altes unnoetiges Statement\npublic class Test\n{";
        var newText = "public class Test\n{";
        var projectFile = new ProjectFile() { FullPath = "/tmp/File.cs", RelativePath = "File.cs" };

        // Act
        var result = _sut.ExtractIssues(projectFile, oldText, newText);

        // Assert
        Assert.That(result, Has.Count.EqualTo(1));
        var issue = result.First();
        Assert.That(
            issue.OriginalLineNumber,
            Is.EqualTo(1),
            "Löschungen in der ersten Zeile müssen bei Zeile 1 anfangen."
        );
        Assert.That(issue.DiffText, Contains.Substring("- // Altes unnoetiges Statement"));
    }
}
