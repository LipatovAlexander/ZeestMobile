using ZeestMobile.ViewModels;

namespace ZeestMobile;

public partial class MainPage : ContentPage
{
    public MainPage(TodoListsViewModel todoListsViewModel)
    {
        InitializeComponent();
        BindingContext = todoListsViewModel;
    }
}