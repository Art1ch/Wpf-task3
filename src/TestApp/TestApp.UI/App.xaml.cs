using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TestApp.UI.ViewModels;
using TestApp.UI.Views;
using TestApp.Application;
using TestApp.Infrastructure;
using TestApp.Infrastructure.Settings;
using CommunityToolkit.Mvvm.Messaging;

namespace TestApp.UI;

public partial class App : System.Windows.Application
{
    private ServiceProvider _serviceProvider = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        ConfigureServices(services);

        _serviceProvider = services.BuildServiceProvider();

        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();

        mainWindow.Show();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services
            .AddTransient<MainWindow>()
            .AddTransient<UserImportWindow>()
            .AddTransient<UserExportWindow>();

        services
            .AddTransient<MainViewModel>()
            .AddTransient<UserImportViewModel>()
            .AddTransient<UserExportViewModel>();

        var userDbSettings = new UserDbSettings();

        services
            .AddApplicationLayer()
            .AddInfrastructureLayer(userDbSettings);

        
        services
            .AddSingleton<IMapper, Mapper>()
            .AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
    }
}

