using CSharpierLinter.Entities;

namespace CSharpierLinter.Interfaces;

public interface ICSharpierScanner
{
    public Task<ScannerResult> ScanAsync();
}
