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

    public string Name { get; } = "Csv parser";
    public string FileExtension { get; } = ".csv";


    public CsvUserParser(ILogger<CsvUserParser> logger)
    {
        _logger = logger;

        _config = new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            Delimiter = ";",
            TrimOptions = TrimOptions.Trim,
            IgnoreBlankLines = true,
            MissingFieldFound = null,
            HeaderValidated = null
        };
    }

    public bool ValidateFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) return false;

        filePath = filePath.Trim();

        if (!File.Exists(filePath)) return false;

        var extension = Path.GetExtension(filePath);
        if (!extension.Equals(".csv", StringComparison.OrdinalIgnoreCase)) return false;

        return true;
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