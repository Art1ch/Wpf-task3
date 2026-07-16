using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using TestApp.UI.Messages;
using TestApp.UI.ViewModels;

namespace TestApp.UI.Views;

public partial class MainWindow : Window
{
    private readonly UserImportWindow _userImportWindow;
    private readonly UserExportWindow _userExportWindow;

    public MainWindow(
        MainViewModel mainViewModel,
        UserImportWindow userImportWindow,
        UserExportWindow userExportWindow
    )
    {
        InitializeComponent();
        DataContext = mainViewModel;

        _userImportWindow = userImportWindow;
        _userExportWindow = userExportWindow;

        ConfigureMessageHandler<OpenUserImportWindowMessage, UserImportWindow>(_userImportWindow);
        ConfigureMessageHandler<OpenUserExportWindowMessage, UserExportWindow>(_userExportWindow);
    }

    private void ConfigureMessageHandler<TMessage, TWindow>(
        TWindow window
    ) 
        where TWindow : Window
        where TMessage : class
    {
        WeakReferenceMessenger.Default.Register<TMessage>(
            this,
            (_, _) =>
            {
                window.Owner = this;
                window.ShowDialog();
            }
        );
    }
}