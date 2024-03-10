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
    }
}