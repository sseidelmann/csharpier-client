namespace CSharpierLinter.Entities;

public class ProjectFile
{
    public string FullPath { get; set; }

    public string RelativePath { get; set; }

    public async Task<string> GetContent()
    {
        return await File.ReadAllTextAsync(FullPath);
    }
}
