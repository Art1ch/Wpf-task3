using CommunityToolkit.Mvvm.Messaging;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Windows;
using TestApp.Application;
using TestApp.Infrastructure;
using TestApp.Infrastructure.Settings;
using TestApp.UI.Services.Implementations;
using TestApp.UI.Services.Interfaces;
using TestApp.UI.ViewModels;
using TestApp.UI.Views;

namespace TestApp.UI;

public partial class App : System.Windows.Application
{
    private IConfiguration _configuration = null!;
    private ServiceProvider _serviceProvider = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        _configuration = builder.Build();

        var services = new ServiceCollection();
        ConfigureServices(services);

        _serviceProvider = services.BuildServiceProvider();

        _serviceProvider.InitializeDatabase();

        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(configure =>
        {
            configure.AddConsole();
            configure.AddDebug();
            configure.SetMinimumLevel(LogLevel.Information);
        });

        services
            .AddTransient<MainWindow>()
            .AddTransient<UserImportWindow>()
            .AddTransient<UserExportWindow>();

        services
            .AddTransient<MainViewModel>()
            .AddTransient<UserImportViewModel>()
            .AddTransient<UserExportViewModel>();

        services
            .AddTransient<IDialogService, DialogService>();

        var userDbSettings = new UserDbSettings();

        _configuration.GetSection(typeof(UserDbSettings).Name).Bind(userDbSettings);

        services
            .AddApplicationLayer()
            .AddInfrastructureLayer(userDbSettings);
        
        services
            .AddSingleton<IMapper, Mapper>()
            .AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
    }
}