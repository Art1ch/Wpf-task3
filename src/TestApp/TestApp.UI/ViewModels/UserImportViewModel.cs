using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FluentValidation;
using MapsterMapper;
using System.Windows;
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
    private readonly IFileDialogService _fileDialogService;
    private readonly IMessenger _messenger;
    private readonly IValidator<UserImportModel> _validator;

    [ObservableProperty]
    private IEnumerable<IUserParser> _allUserParsers = [];

    [ObservableProperty]
    private IUserParser _selectedUserParser;

    [ObservableProperty]
    private string _status = "Ready to import";

    [ObservableProperty]
    private bool _isImporting;

    [ObservableProperty]
    private int _importedCount;

    [ObservableProperty]
    private string? _selectedFilePath;

    public UserImportViewModel(
        IEnumerable<IUserParser> parsers,
        IUserRepository repository,
        IFileDialogService fileDialogService,
        IMapper mapper,
        IValidator<UserImportModel> validator,
        IMessenger messenger)
    {
        AllUserParsers = parsers;

        _repository = repository;
        _fileDialogService = fileDialogService;
        _mapper = mapper;
        _validator = validator;
        _messenger = messenger;

        SelectedUserParser = AllUserParsers.FirstOrDefault()!; 
    }

    [RelayCommand]
    private async Task ImportAsync()
    {

        var filePath = _fileDialogService.GetFilePath(SelectedUserParser.FileExtension, "Select file");

        if (filePath == null) return;

        SelectedFilePath = filePath;

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
            var batchSize = 1000;
            var batchReadCount = 0;

            var userList = new List<UserEntity>(batchSize);

            await foreach (var userModel in SelectedUserParser.ParseInBatchesAsync(SelectedFilePath, batchSize))
            {
                if (batchReadCount >= 1000)
                {
                    await _repository.InsertBulkOfUsersAsync(userList);

                    ImportedCount += userList.Count;
                    batchReadCount = 0;

                    userList.Clear();
                }

                batchReadCount++;

                if (_validator.Validate(userModel).IsValid)
                {
                    var userEntity = _mapper.Map<UserEntity>(userModel);
                    userList.Add(userEntity);
                }
            }

            if (userList.Count > 0)
            {
                await _repository.InsertBulkOfUsersAsync(userList);
                ImportedCount += userList.Count;
            }

            Status = $"Import completed: {ImportedCount} users";
        }
        catch (Exception ex)
        {
            Status = $"Error: {ex.Message}";
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsImporting = false;
        }
    }
}