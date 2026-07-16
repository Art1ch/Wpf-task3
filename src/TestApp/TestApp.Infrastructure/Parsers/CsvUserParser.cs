using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Runtime.CompilerServices;
using TestApp.Application.Abstractions;
using TestApp.Core.Models;

namespace TestApp.Infrastructure.Parsers;

public class CsvUserParser : IUserParser
{
    private readonly ILogger<CsvUserParser> _logger;
    private readonly CsvConfiguration _config;

    public string FileExtension { get; } = ".csv";

    public CsvUserParser(ILogger<CsvUserParser> logger)
    {
        _logger = logger;

        _config = new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ";",
            TrimOptions = TrimOptions.Trim,
            IgnoreBlankLines = true,
            MissingFieldFound = null,
            HeaderValidated = null
        };
    }

    public bool ValidateFile(string filePath)
    {
        return !string.IsNullOrWhiteSpace(filePath) &&
               File.Exists(filePath) &&
               Path.GetExtension(filePath).Equals(".csv", StringComparison.OrdinalIgnoreCase) &&
               new FileInfo(filePath).Length > 0;
    }

    public async IAsyncEnumerable<UserImportModel> ParseInBatchesAsync(
        string filePath,
        int batchSize = 1000,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var stream = File.OpenRead(filePath);
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, _config);

        var batch = new List<UserImportModel>(batchSize);

        await csv.ReadAsync();
        csv.ReadHeader();

        while (await csv.ReadAsync())
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (batch.Count >= batchSize)
            {
                foreach (var item in batch)
                    yield return item;

                batch.Clear();
            }

            if (batch.Count > 0)
            {
                foreach (var item in batch)
                    yield return item;
            }

            try
            {
                var model = new UserImportModel
                {
                    DataCollectedDate = DateOnly.Parse(csv.GetField(0)!),
                    FirstName = csv.GetField(1) ?? string.Empty,
                    LastName = csv.GetField(2) ?? string.Empty,
                    MiddleName = csv.GetField(3) ?? string.Empty,
                    City = csv.GetField(4) ?? string.Empty,
                    Country = csv.GetField(5) ?? string.Empty,
                };

                batch.Add(model);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}