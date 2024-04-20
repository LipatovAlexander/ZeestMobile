using ZeestMobile.Services;

namespace ZeestMobile.Pages;

public partial class SettingsPage : ContentPage
{
    private readonly Synchronizer _synchronizer;

    public SettingsPage(Synchronizer synchronizer)
    {
        _synchronizer = synchronizer;
        InitializeComponent();
    }

    private async void Button_OnClicked(object? sender, EventArgs e)
    {
        await _synchronizer.SyncAsync();
    }
}