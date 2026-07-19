using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FluentValidation;
using MapsterMapper;
using TestApp.Application.Abstractions;
using TestApp.Core.Entities;
using TestApp.Core.Models;
using TestApp.UI.Messages;
using TestApp.UI.Services.Interfaces;

namespace TestApp.UI.ViewModels;

public partial class UserImportViewModel : ObservableObject
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly IDialogService _dialogService;
    private readonly IMessenger _messenger;
    private readonly IValidator<UserImportModel> _validator;

    [ObservableProperty]
    private IEnumerable<IUserParser> _allUserParsers = [];

    [ObservableProperty]
    private IUserParser _selectedUserParser;

    [ObservableProperty]
    private string _status = "Ready to import";

    [ObservableProperty]
    private bool _isImporting = false;

    [ObservableProperty]
    private int _importedCount;

    [ObservableProperty]
    private string? _selectedFilePath;

    public UserImportViewModel(
        IEnumerable<IUserParser> parsers,
        IUserRepository repository,
        IDialogService fileDialogService,
        IMapper mapper,
        IValidator<UserImportModel> validator,
        IMessenger messenger)
    {
        AllUserParsers = parsers;

        _repository = repository;
        _dialogService = fileDialogService;
        _mapper = mapper;
        _validator = validator;
        _messenger = messenger;

        SelectedUserParser = AllUserParsers.FirstOrDefault()!; 
    }

    [RelayCommand]
    public async Task Import()
    {
        if (string.IsNullOrEmpty(SelectedFilePath)) return;

        if (!SelectedUserParser.ValidateFile(SelectedFilePath))
        {
            Status = "Invalid file";
            _messenger.Send(new ShowErrorMessage("File didn't pass validation"));
            return;
        }

        IsImporting = true;
        Status = "Importing...";

        try
        {
            var batchSize = 10000;

            await foreach (var batch in SelectedUserParser.ParseInBatchesAsync(SelectedFilePath, batchSize))
            {
                var userModels = new List<UserImportModel>(batchSize);

                foreach (var model in batch)
                {
                    if (!_validator.Validate(model).IsValid)
                        continue;

                    userModels.Add(model);
                }

                if (userModels.Count == 0)
                    continue;

                var userToBulk = _mapper.Map<List<UserEntity>>(userModels);
                await _repository.InsertBulkOfUsersAsync(userToBulk, userToBulk.Count);

                ImportedCount += userToBulk.Count;
            }

            Status = $"Import completed: {ImportedCount} users";
        }
        catch (Exception ex)
        {
            Status = $"Error while parsing the file";
            _messenger.Send(new ShowErrorMessage($"Error {ex.Message}"));
        }
        finally
        {
            IsImporting = false;
        }
    }

    [RelayCommand]
    public async Task ChooseFilePath()
    {
        var filePath = _dialogService.GetFilePath(SelectedUserParser.FileExtension, "Select file");

        if (filePath == null) return;

        SelectedFilePath = filePath;
    }
}