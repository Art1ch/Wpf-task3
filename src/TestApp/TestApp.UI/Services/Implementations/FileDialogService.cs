using Microsoft.Win32;
using TestApp.UI.Services.Interfaces;

namespace TestApp.UI.Services.Implementations;

internal sealed class FileDialogService : IFileDialogService
{
    public string? GetFilePath(string fileExtension, string title)
    {
        var dialog = new OpenFileDialog
        {
            Filter = $"*{fileExtension}",
            Title = title,
        };

        return dialog.ShowDialog() == true
            ? dialog.FileName
            : null;
    }
}
