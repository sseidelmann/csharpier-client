namespace CsharpierCodeInsights.Constants;

/// <summary>
/// <see cref="https://support.atlassian.com/bitbucket-cloud/kb/convert-bitbucket-pipeline-test-reports-into-pull-requests-code-insight-reports/"/>
/// </summary>
public static class BitbucketAnnotationType
{
    /// <summary>
    /// Defines the annotation type "CODE_SMELL"
    /// </summary>
    public const string CodeSmell = "CODE_SMELL";

    /// <summary>
    /// Defines the annotation type "VULNERABILITY"
    /// </summary>
    public const string Vulnerability = "VULNERABILITY";

    /// <summary>
    /// Defines the annotation type "BUG"
    /// </summary>
    public const string Bug = "BUG";
}
