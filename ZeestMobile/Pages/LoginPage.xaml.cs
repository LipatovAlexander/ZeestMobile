using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeestMobile.Services;

namespace ZeestMobile.Pages;

public partial class LoginPage : ContentPage
{
    private readonly Synchronizer _synchronizer;

    public LoginPage(Synchronizer synchronizer)
    {
        _synchronizer = synchronizer;
        InitializeComponent();
    }
    
    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(usernameEntry.Text))
        {
            Preferences.Set("username", usernameEntry.Text);
            
            Application.Current.MainPage = new AppShell();

           await _synchronizer.SyncAsync();
        }
    }
}