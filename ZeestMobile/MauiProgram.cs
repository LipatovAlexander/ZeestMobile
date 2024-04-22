using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZeestMobile.Infrastructure.EntityFramework;
using ZeestMobile.Infrastructure.Migrations;
using ZeestMobile.Pages;
using ZeestMobile.Services;
using ZeestMobile.ViewModels;

namespace ZeestMobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "zeest.db");
        var connectionString = $"FileName={dbPath}";
        Console.WriteLine(connectionString);
        builder.Services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseSqlite(connectionString);
        }, ServiceLifetime.Singleton);

        builder.Services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddSQLite()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(InitMigration).Assembly).For.Migrations());
#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<ToDoListsPage>();
        builder.Services.AddSingleton<TodoListsViewModel>();
        builder.Services.AddSingleton<SettingsPage>();
        builder.Services.AddSingleton<Synchronizer>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<App>();
        
        var app = builder.Build();

        using var scope = app.Services.CreateScope();

        scope.ServiceProvider
            .GetRequiredService<IMigrationRunner>()
            .MigrateUp();
        
        return app;
    }
}