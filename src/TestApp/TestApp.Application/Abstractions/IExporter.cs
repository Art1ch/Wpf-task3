using TestApp.Core.Models;

namespace TestApp.Application.Abstractions;

public interface IExporter
{
    public string FormatName { get; set; }
    public string FileExtension { get; set; }
    void Export(IEnumerable<UserExportModel> users, string filePath);
}