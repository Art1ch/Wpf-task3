using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using TestApp.UI.Messages;
using TestApp.UI.ViewModels;

namespace TestApp.UI.Views;

public partial class UserImportWindow : Window
{
    public UserImportWindow(UserImportViewModel userImportViewModel)
    {
        InitializeComponent();
        DataContext = userImportViewModel;

        ConfigureMessageHandlers();
    }

    private void ConfigureMessageHandlers()
    {
        WeakReferenceMessenger.Default.Register<ShowErrorMessage>(
           this,
           (_, message) =>
           {
               MessageBox.Show(
                   message.Message,
                   "Error",
                   MessageBoxButton.OK,
                   MessageBoxImage.Error);
           }
        );
    }
}
