namespace TestApp.UI.Services.Interfaces;

public interface IFileDialogService
{
    string? GetFilePath(string fileExtension, string title);
}
