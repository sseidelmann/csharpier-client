namespace CSharpierLinter.Entities;

public class ScannerResult
{
    public List<ScannedFileResult> ScannedFileResults { get; set; }

    public int TotalScannedFiles { get; init; }

    public int TotalIssues { get; init; }

    public double IssuesPerFile { get; init; }

    public int FilesWithIssues { get; init; }

    public TimeSpan RunTime { get; set; }

    public ScannerResult(List<ScannedFileResult> scannedFileResults, TimeSpan runTime)
    {
        ScannedFileResults = scannedFileResults;
        TotalScannedFiles = scannedFileResults.Count;
        TotalIssues = scannedFileResults.Sum(r => r.IssueList.Count);
        IssuesPerFile = TotalIssues / TotalScannedFiles;
        FilesWithIssues = scannedFileResults.Count(r => r.IssueList.Count > 0);
        RunTime = runTime;
    }
}
