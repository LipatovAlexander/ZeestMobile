namespace ZeestMobile.Pages;

public partial class OnboardingPage : ContentPage
{
    public bool IsChecked { get; set; }
    public OnboardingPage()
    {
        InitializeComponent();
        
        IsChecked = Preferences.Get("skip_onboarding", false);

        BindingContext = this;
    }

    private void CheckBox_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        Preferences.Set("skip_onboarding", e.Value);
    }
}