using Microsoft.Win32;
using TestApp.UI.Services.Interfaces;

namespace TestApp.UI.Services.Implementations;

internal sealed class DialogService : IDialogService
{
    public string? GetFilePath(string fileExtension, string title)
    {
        var dialog = new OpenFileDialog
        {
            Filter = $"{fileExtension} files (*{fileExtension})|*{fileExtension}|All files (*.*)|*.*",
            Title = title,
        };

        return dialog.ShowDialog() == true
            ? dialog.FileName
            : null;
    }

    public string? GetFolderPath(string title)
    {
        var dialog = new OpenFolderDialog
        {
            Title = title,
        };

        return dialog.ShowDialog() == true
            ? dialog.FolderName
            : null;
    }
}
