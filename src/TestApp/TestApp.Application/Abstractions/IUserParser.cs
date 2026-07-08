using TestApp.Core.Models;

namespace TestApp.Application.Abstractions;

public interface IUserParser
{
    bool ValidateFile(string filePath);
    IAsyncEnumerable<UserImportModel> ParseInBatchesAsync(
        string filePath,
        int batchSize = 1000,
        CancellationToken cancellationToken = default
    );
}
