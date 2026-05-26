namespace CsharpierCodeInsights.Constants;

/// <summary>
/// <see cref="https://support.atlassian.com/bitbucket-cloud/kb/convert-bitbucket-pipeline-test-reports-into-pull-requests-code-insight-reports/"/>
/// </summary>
public static class BitbucketResult
{
    /// <summary>
    /// Defines the result "PASSED"
    /// </summary>
    public const string Passed = "PASSED";

    /// <summary>
    /// Defines the result "FAILED"
    /// </summary>
    public const string Failed = "FAILED";

    /// <summary>
    /// Defines the result "PENDING"
    /// </summary>
    public const string Pending = "PENDING";
}
