using TestApp.Core.Models;

namespace TestApp.Application.Abstractions;

public interface IUserParser
{
    public string Name { get; }
    public string FileExtension { get; }
    bool ValidateFile(string filePath);
    IAsyncEnumerable<UserImportModel> ParseInBatchesAsync(
        string filePath,
        int batchSize = 1000,
        CancellationToken cancellationToken = default
    );
}
