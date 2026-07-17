namespace TestApp.UI.Services.Interfaces;

public interface IDialogService
{
    string? GetFilePath(string fileExtension, string title);
    string? GetFolderPath(string title);
}
