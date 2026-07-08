namespace TestApp.Application.Abstractions;

public interface IDialogService
{
    string? OpenFileDialog(string? filter = null, string? title = null);
    string? SaveFileDialog(string? defaultExtension = null, string? filter = null, string? title = null);
    string? SelectFolderDialog(string? title = null);
    void ShowMessage(string message, string title = "Информация", bool isError = false);
    bool ShowConfirmation(string message, string title = "Подтверждение");
    bool? ShowQuestion(string message, string title = "Вопрос");
    string? ShowInputDialog(string message, string title = "Ввод", string defaultValue = "");
    void ShowProgressDialog(string title, Action<IProgress<double>> action);
    T? ShowSelectionDialog<T>(IEnumerable<T> items, string title = "Выбор", string displayMember = "");
}