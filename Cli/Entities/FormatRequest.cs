using System.Text.Json.Serialization;

namespace Cgnd.CSharpier.Cli.Entities;

/// <summary>
/// Model for the formatting request
/// </summary>
public class FormatRequest
{
    /// <summary>
    /// The code.
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;
    
    /// <summary>
    /// The line length to provide
    /// </summary>
    [JsonPropertyName("printWidth")]
    public int PrintWidth { get; set; } = 100;
    
    /// <summary>
    /// The indention size
    /// </summary>
    [JsonPropertyName("indentSize")]
    public int IndentSize { get; set; } = 4;
    
    /// <summary>
    /// Using tabs?
    /// </summary>
    [JsonPropertyName("useTabs")]
    public bool UseTabs { get; set; } = false;
    
    /// <summary>
    /// Formatter to choose
    /// </summary>
    [JsonPropertyName("formatter")]
    public string Formatter { get; set; } = "CSharp";
}