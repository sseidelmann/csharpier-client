using CSharpierLinter.Entities;

namespace CSharpierLinter.Interfaces;

public interface IOutputWriter
{
    Task Write(ScannerResult scannerResult);
}
