using ZeestMobile.Pages;

namespace ZeestMobile;

public partial class App : Application
{
    public App(LoginPage loginPage)
    {
        InitializeComponent();

        string username = Preferences.Get("username", null);
        if(username != null)
        {
            // Если пользователь уже авторизован, переводим его на главную страницу
            MainPage = new AppShell();
        }
        else
        {
            // Отображаем страницу входа
            MainPage = loginPage;
        }
    }

    protected override async void OnStart()
    {
        string username = Preferences.Get("username", null);
        if(username != null && !Preferences.Get("skip_onboarding", false))
        {
            // Если пользователь уже авторизован, переводим его на главную страницу
            Shell.Current.GoToAsync("//Onboarding").GetAwaiter().GetResult();

        }

        if (!Preferences.ContainsKey("synced_at"))
        {
            Preferences.Set("synced_at", DateTime.MinValue);
        }
    }
}