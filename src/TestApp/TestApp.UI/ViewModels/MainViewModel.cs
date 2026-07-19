using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TestApp.UI.Messages;

namespace TestApp.UI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IMessenger _messenger;

    public MainViewModel(IMessenger messenger)
    {
        _messenger = messenger;
    }

    [RelayCommand]
    private void OpenImport()
    {
        _messenger.Send(new OpenUserImportWindowMessage());
    }

    [RelayCommand]
    private void OpenExport()
    {
        _messenger.Send(new OpenUserExportWindowMessage());
    }
}