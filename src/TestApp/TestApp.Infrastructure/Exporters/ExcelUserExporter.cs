using ClosedXML.Excel;
using TestApp.Application.Abstractions;
using TestApp.Core.Models;

namespace TestApp.Infrastructure.Exporters;

internal sealed class ExcelUserExporter : IUserExporter
{
    public string Name { get; } = "Excel exporter";
    public string FileExtension { get; } = ".xlsx";

    public void Export(IEnumerable<UserExportModel> users, string folderPath)
    {
        if (users == null || !users.Any())
            throw new ArgumentException("No data for export");

        Directory.CreateDirectory(folderPath);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Users");

        SetupHeaders(worksheet);

        int row = 2;

        foreach (var user in users)
        {
            worksheet.Cell(row, 1).Value = user.Id.ToString();
            worksheet.Cell(row, 2).Value = user.FirstName;
            worksheet.Cell(row, 3).Value = user.LastName;
            worksheet.Cell(row, 4).Value = user.MiddleName;
            worksheet.Cell(row, 5).Value = user.City;
            worksheet.Cell(row, 6).Value = user.Country;
            worksheet.Cell(row, 7).Value = user.DataCollectedDate.ToShortDateString();
            row++;
        }

        worksheet.Columns().AdjustToContents();

        var fileName = $"Users_{DateTime.Now:yyyyMMdd_HHmmss}{FileExtension}";
        var filePath = Path.Combine(folderPath, fileName);

        workbook.SaveAs(filePath);
    }

    private void SetupHeaders(IXLWorksheet worksheet)
    {
        string[] headers = { "Id", "First name", "Last name", "Middle name", "City", "Country", "Data collected date" };
        for (int i = 0; i < headers.Length; i++)
        {
            var cell = worksheet.Cell(1, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontSize = 12;
            cell.Style.Fill.BackgroundColor = XLColor.LightGray;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }
    }
}
