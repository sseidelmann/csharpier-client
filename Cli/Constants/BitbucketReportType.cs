namespace CsharpierCodeInsights.Constants;

/// <summary>
/// <see cref="https://support.atlassian.com/bitbucket-cloud/kb/convert-bitbucket-pipeline-test-reports-into-pull-requests-code-insight-reports/"/>
/// </summary>
public static class BitbucketReportType
{
    /// <summary>
    /// Defines the report type "SECURITY"
    /// </summary>
    public const string Security = "SECURITY";

    /// <summary>
    /// Defines the report type "COVERAGE"
    /// </summary>
    public const string Coverage = "COVERAGE";

    /// <summary>
    /// Defines the report type "TEST"
    /// </summary>
    public const string Test = "TEST";

    /// <summary>
    /// Defines the report type "BUG"
    /// </summary>
    public const string Bug = "BUG";
}
