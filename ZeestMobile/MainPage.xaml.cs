using Microsoft.EntityFrameworkCore;
using ZeestMobile.Infrastructure.EntityFramework;
using ZeestMobile.Pages;
using ZeestMobile.Services;

namespace ZeestMobile;

public partial class MainPage : ContentPage
{
    private readonly LoginPage _loginPage;
    private readonly ApplicationContext _applicationContext;
    private readonly Synchronizer _synchronizer;

    public MainPage(LoginPage loginPage, ApplicationContext applicationContext, Synchronizer synchronizer)
    {
        _loginPage = loginPage;
        _applicationContext = applicationContext;
        _synchronizer = synchronizer;
        InitializeComponent();
    }

    private async void Logout(object? sender, EventArgs e)
    {
        Preferences.Clear("username");

        // Меняем MainPage или навигируем к другой странице
        Application.Current.MainPage = _loginPage;
        
        Preferences.Set("synced_at", DateTime.MinValue);

       await _applicationContext.TodoLists
            .ExecuteDeleteAsync();
       _applicationContext.ChangeTracker.Clear();
    }
}