using CsharpierCodeInsights.Cli;
using CSharpierLinter.Entities;
using CSharpierLinter.Interfaces;

namespace CSharpierLinter.Business;

public class FileLocator : IFileLocator
{
    private readonly CliHandler _cliHandler;

    public FileLocator(CliHandler cliHandler)
    {
        _cliHandler = cliHandler;
    }

    public List<ProjectFile> GetFilesForCheck()
    {
        var projectFolder = _cliHandler.GetWorkingDirectory();
        var fullPath = Path.GetFullPath(projectFolder);
        return Directory
            .EnumerateFiles(fullPath, "*.*", SearchOption.AllDirectories)
            .Where(path =>
                !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}")
                && !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}")
                && (path.EndsWith(".cs") || path.EndsWith(".csproj"))
                && !path.EndsWith("Tests.cs")
            )
            .Select(file =>
            {
                return new ProjectFile()
                {
                    FullPath = file,
                    RelativePath = Path.GetRelativePath(fullPath, file).Replace('\\', '/'),
                };
            })
            .ToList();
    }
}
