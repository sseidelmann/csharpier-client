using System.Text.Json.Serialization;
using CSharpierLinter.Entities;

namespace Cgnd.CSharpier.Cli.Entities;

public class FormatResponse
{
    [JsonPropertyName("code")]
    public required string Code { get; set; }
    
    [JsonPropertyName("json")]
    public required string Json { get; set; }
    
    [JsonPropertyName("doc")]
    public required string Doc { get; set; }
    
    [JsonPropertyName("errors")]
    public required List<FormatResponseError> Errors { get; set; }
    
    [JsonPropertyName("syntaxValidation")]
    public required string SyntaxValidation { get; set; }
}