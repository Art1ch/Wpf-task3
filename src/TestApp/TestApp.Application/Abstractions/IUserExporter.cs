using TestApp.Core.Models;

namespace TestApp.Application.Abstractions;

public interface IUserExporter
{
    public string Name { get; }
    public string FileExtension { get; }
    void Export(IEnumerable<UserExportModel> users, string folderPath);
}