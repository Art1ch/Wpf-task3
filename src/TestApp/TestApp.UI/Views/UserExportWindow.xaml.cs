using System.Windows;
using TestApp.UI.ViewModels;

namespace TestApp.UI.Views;

public partial class UserExportWindow : Window
{
    public UserExportWindow(UserExportViewModel userExportViewModel)
    {
        InitializeComponent();
        DataContext = userExportViewModel;
    }
}
