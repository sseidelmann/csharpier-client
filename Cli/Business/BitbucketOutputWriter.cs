using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CsharpierCodeInsights.Constants;
using CSharpierLinter.Entities;
using CSharpierLinter.Interfaces;

namespace CsharpierCodeInsights.Business;

public class BitbucketOutputWriter : IOutputWriter
{
    private readonly HttpClient _httpClient;
    private readonly string _repoOwner;
    private readonly string _repoSlug;
    private readonly string _commit;
    private readonly string _reportId = "csharpier-linter-report";

    public BitbucketOutputWriter(IHttpClientFactory httpClientFactory)
    {
        _repoOwner = GetBitbucketEnvironmentVariable("REPO_OWNER");
        _repoSlug = GetBitbucketEnvironmentVariable("REPO_SLUG");
        _commit = GetBitbucketEnvironmentVariable("COMMIT");
        var user = GetBitbucketEnvironmentVariable("USER");
        var appPassword = GetBitbucketEnvironmentVariable("APP_PASSWORD");

        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://api.bitbucket.org/2.0/");
        var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{user}:{appPassword}"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic",
            authToken
        );
    }

    private static string GetBitbucketEnvironmentVariable(string variableName)
    {
        return Environment.GetEnvironmentVariable($"BITBUCKET_{variableName}")
            ?? throw new InvalidOperationException(
                $"Environment variable BITBUCKET_{variableName} is not set."
            );
    }

    public async Task Write(ScannerResult scannerResult)
    {
        var report = new BitbucketCodeInsightReport
        {
            Title = "CSharpier Linter",
            Details = "Code formatting check by CSharpier CodeInsights.",
            ReportType = BitbucketReportType.Bug,
            Reporter = "CSharpierCodeInsights",
            Result =
                scannerResult.TotalIssues > 0 ? BitbucketResult.Failed : BitbucketResult.Passed,
            Data = new List<BitbucketReportData>
            {
                new()
                {
                    Title = "Total Scanned Files",
                    Value = scannerResult.TotalScannedFiles,
                    Type = BitbucketReportDataType.Number,
                },
                new()
                {
                    Title = "Total Issues",
                    Value = scannerResult.TotalIssues,
                    Type = BitbucketReportDataType.Number,
                },
                new()
                {
                    Title = "Files With Issues",
                    Value = scannerResult.FilesWithIssues,
                    Type = BitbucketReportDataType.Number,
                },
            },
        };

        var reportUrl =
            $"repositories/{_repoOwner}/{_repoSlug}/commit/{_commit}/reports/{_reportId}";
        var jsonOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var reportJson = JsonSerializer.Serialize(report, jsonOptions);
        var reportContent = new StringContent(reportJson, Encoding.UTF8, "application/json");

        var reportResponse = await _httpClient.PutAsync(reportUrl, reportContent);
        reportResponse.EnsureSuccessStatusCode();
        Console.WriteLine("Successfully created Bitbucket CodeInsight report.");

        var annotations = scannerResult
            .ScannedFileResults.SelectMany(a =>
            {
                var list = new List<BitbucketAnnotation>();
                foreach (var issue in a.IssueList)
                {
                    list.Add(
                        new BitbucketAnnotation
                        {
                            Path = a.ProjectFile.RelativePath,
                            Line = issue.OriginalLineNumber,
                            Message = issue.DiffText,
                            Severity = BitbucketAnnotationSeverity.Low,
                            Type = BitbucketAnnotationType.CodeSmell,
                        }
                    );
                }

                return list;
            })
            .ToList();

        if (annotations.Any())
        {
            // First, delete existing annotations
            var deleteAnnotationsResponse = await _httpClient.DeleteAsync(
                $"{reportUrl}/annotations"
            );
            deleteAnnotationsResponse.EnsureSuccessStatusCode();

            // Post new annotations in batches of 100
            const int batchSize = 100;
            for (var i = 0; i < annotations.Count; i += batchSize)
            {
                var batch = annotations.Skip(i).Take(batchSize).ToList();
                var annotationsJson = JsonSerializer.Serialize(
                    new { annotations = batch },
                    jsonOptions
                );
                var annotationsContent = new StringContent(
                    annotationsJson,
                    Encoding.UTF8,
                    "application/json"
                );

                var annotationsResponse = await _httpClient.PostAsync(
                    $"{reportUrl}/annotations",
                    annotationsContent
                );
                annotationsResponse.EnsureSuccessStatusCode();
            }
            Console.WriteLine($"Successfully posted {annotations.Count} annotations to Bitbucket.");
        }
    }
}

public class BitbucketCodeInsightReport
{
    public string Title { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string ReportType { get; set; } = string.Empty;
    public string Reporter { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    public List<BitbucketReportData> Data { get; set; } = new();
}

public class BitbucketReportData
{
    public string Title { get; set; } = string.Empty;
    public object Value { get; set; } = new();
    public string Type { get; set; } = string.Empty;
}

public class BitbucketAnnotation
{
    public string Path { get; set; } = string.Empty;
    public int Line { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}
