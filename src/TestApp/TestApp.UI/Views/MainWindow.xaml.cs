using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TestApp.UI.Messages;
using TestApp.UI.ViewModels;

namespace TestApp.UI.Views;

public partial class MainWindow : Window
{
    private readonly IServiceProvider _serviceProvider;

    public MainWindow(
        MainViewModel mainViewModel,
        IServiceProvider serviceProvider
    )
    {
        InitializeComponent();
        DataContext = mainViewModel;

        _serviceProvider = serviceProvider;

        WeakReferenceMessenger.Default.Register<OpenUserImportWindowMessage>(this, OnOpenImportWindow);
        WeakReferenceMessenger.Default.Register<OpenUserExportWindowMessage>(this, OnOpenExportWindow);
    }

    private void OnOpenImportWindow(object recipient, OpenUserImportWindowMessage message)
    {
        var window = _serviceProvider.GetRequiredService<UserImportWindow>();
        window.Owner = this;
        window.ShowDialog();
    }

    private void OnOpenExportWindow(object recipient, OpenUserExportWindowMessage message)
    {
        var window = _serviceProvider.GetRequiredService<UserExportWindow>();
        window.Owner = this;
        window.ShowDialog();
    }

    protected override void OnClosed(EventArgs e)
    {
        WeakReferenceMessenger.Default.Unregister<OpenUserImportWindowMessage>(this);
        WeakReferenceMessenger.Default.Unregister<OpenUserExportWindowMessage>(this);
        base.OnClosed(e);
    }
}