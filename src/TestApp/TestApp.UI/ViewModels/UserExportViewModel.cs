using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using TestApp.Application.Abstractions;
using TestApp.Application.Filters;
using TestApp.Core.Models;
using TestApp.UI.Messages;
using TestApp.UI.Services.Interfaces;

namespace TestApp.UI.ViewModels;

public partial class UserExportViewModel : ObservableObject
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly IDialogService _dialogService;
    private readonly IMessenger _messenger;
    private readonly ILogger<UserExportViewModel> _logger;

    [ObservableProperty]
    private IEnumerable<IUserExporter> _allUserExporters = [];

    [ObservableProperty]
    private IUserExporter _selectedUserExporter;

    [ObservableProperty]
    private string _status = "Ready to export";

    [ObservableProperty]
    private bool _isExporting;

    [ObservableProperty]
    private int _exportedCount;

    [ObservableProperty]
    private string? _selectedFolderPath;

    [ObservableProperty]
    private UserFilter _userFilter = new UserFilter();

    public UserExportViewModel(
        IEnumerable<IUserExporter> allUserExporters,
        IUserRepository repository,
        IMapper mapper,
        IDialogService dialogService,
        IMessenger messenger,
        ILogger<UserExportViewModel> logger)
    {
        var exportersList = allUserExporters.ToList();
        logger.LogInformation($"Received {exportersList.Count} exporters");

        foreach (var exporter in exportersList)
        {
            logger.LogInformation($"Exporter: {exporter.GetType().Name}, Name: {exporter.Name}");
        }

        AllUserExporters = allUserExporters;

        _repository = repository;
        _mapper = mapper;
        _dialogService = dialogService;
        _messenger = messenger;

        SelectedUserExporter = allUserExporters.FirstOrDefault()!;
        _logger = logger;
    }

    [RelayCommand]
    public async Task Export()
    {
        if (string.IsNullOrEmpty(SelectedFolderPath)) return;

        IsExporting = true;
        Status = "Exporting...";

        var usersEntitiesToExport = await _repository.GetUsersByFilterAsync(UserFilter);

        var usersToExport = _mapper.Map<List<UserExportModel>>(usersEntitiesToExport);

        try
        {
            SelectedUserExporter.Export(usersToExport, SelectedFolderPath);

            Status = "Export completed";
        }
        catch (Exception ex) 
        {
            Status = "Export failed";
            _messenger.Send(new ShowErrorMessage($"Error {ex.Message}"));
        }
        finally
        {
            IsExporting = false;
        }
    }

    [RelayCommand]
    public async Task ChooseFolderPath()
    {
        var folderPath = _dialogService.GetFolderPath("Select folder");

        if (folderPath == null) return;

        SelectedFolderPath = folderPath;
    }
}
