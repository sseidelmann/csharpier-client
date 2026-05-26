using System.Diagnostics;
using Cgnd.CSharpier.Cli.Entities;
using CsharpierCodeInsights;
using CSharpierLinter.Entities;
using CSharpierLinter.Interfaces;
using DiffPlex.DiffBuilder.Model;

namespace CSharpierLinter.Business;

public class CSharpierScanner : ICSharpierScanner
{
    private readonly IFileLocator _fileLocator;

    private readonly ICodeNormalizer _codeNormalizer;

    private readonly ICSharpierServerClient _client;

    private readonly IIssueExtractor _issueExtractor;

    public CSharpierScanner(
        IFileLocator fileLocator,
        ICodeNormalizer codeNormalizer,
        ICSharpierServerClient client,
        IIssueExtractor issueExtractor
    )
    {
        _fileLocator = fileLocator;
        _codeNormalizer = codeNormalizer;
        _client = client;
        _issueExtractor = issueExtractor;
    }

    public async Task<ScannerResult> ScanAsync()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        var csharpFiles = _fileLocator.GetFilesForCheck();

        var scannedFileResults = new List<ScannedFileResult>();

        foreach (var file in csharpFiles)
        {
            var scannedFileResult = new ScannedFileResult { ProjectFile = file };

            var originalCode = await file.GetContent();
            var payload = new FormatRequest()
            {
                Code = originalCode
            };

            var result = await _client.FormatAsync(payload);

            if (result == null || result.Errors.Count > 0 || result.SyntaxValidation.Length > 0)
            {
                
                continue;
            }

            string normalizedOriginal = _codeNormalizer.NormalizeCode(originalCode);
            string normalizedFormatted = _codeNormalizer.NormalizeCode(result.Code);

            if (normalizedFormatted != normalizedOriginal)
            {
                var issues = _issueExtractor.ExtractIssues(
                    file,
                    normalizedOriginal,
                    normalizedFormatted
                );

                scannedFileResult.IssueList.AddRange(issues);
            }

            scannedFileResults.Add(scannedFileResult);
        }

        stopwatch.Stop();
        
        
        return new ScannerResult(scannedFileResults, stopwatch.Elapsed);
    }
}
