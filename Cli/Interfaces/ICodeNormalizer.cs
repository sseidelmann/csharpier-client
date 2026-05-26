namespace CSharpierLinter.Interfaces;

/// <summary>
/// Code normalizer.
/// </summary>
public interface ICodeNormalizer
{
    /// <summary>
    /// Normalizes the code.
    /// </summary>
    /// <param name="code">the code</param>
    /// <returns>The normalized code</returns>
    public string NormalizeCode(string code);
}
