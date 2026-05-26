using CSharpierLinter.Entities;

namespace CSharpierLinter.Interfaces;

public interface IFileLocator
{
    public List<ProjectFile> GetFilesForCheck();
}
