using ZeestMobile.Pages;

namespace ZeestMobile;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }

    protected override async void OnStart()
    {
        if (!Preferences.Get("skip_onboarding", false))
        {
            Shell.Current.GoToAsync("//Onboarding").GetAwaiter().GetResult();
        }

        if (!Preferences.ContainsKey("user_id"))
        {
            Preferences.Set("user_id", Guid.NewGuid().ToString());
        }

        if (!Preferences.ContainsKey("synced_at"))
        {
            Preferences.Set("synced_at", DateTime.MinValue);
        }
    }
}