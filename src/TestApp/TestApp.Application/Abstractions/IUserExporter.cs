using TestApp.Core.Models;

namespace TestApp.Application.Abstractions;

public interface IUserExporter
{
    public string ExporterName { get; set; }
    public string FileExtension { get; set; }
    void Export(IEnumerable<UserExportModel> users, string filePath);
}