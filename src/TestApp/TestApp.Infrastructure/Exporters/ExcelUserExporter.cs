using ClosedXML.Excel;
using TestApp.Application.Abstractions;
using TestApp.Core.Models;

namespace TestApp.Infrastructure.Exporters;

internal sealed class ExcelUserExporter : IUserExporter
{
    public string ExporterName { get; } = "Excel exporter";
    public string FileExtension { get; } = ".xlsx";

    public void Export(IEnumerable<UserExportModel> users, string filePath)
    {
        if (users == null || !users.Any())
            throw new ArgumentException("No data for export");

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
            row++;
        }

        worksheet.Columns().AdjustToContents();

        workbook.SaveAs(filePath);
    }

    private void SetupHeaders(IXLWorksheet worksheet)
    {
        string[] headers = { "Id", "First name", "Last name", "Middle name", "City", "Country" };
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
