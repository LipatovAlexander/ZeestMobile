using ZeestMobile.ViewModels;

namespace ZeestMobile;

public partial class MainPage : ContentPage
{
    private readonly TodoListsViewModel _todoListsViewModel;
    
    public MainPage(TodoListsViewModel todoListsViewModel)
    {
        InitializeComponent();
        BindingContext = todoListsViewModel;
        _todoListsViewModel = todoListsViewModel;
    }

    private void TodoList_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var text = e.NewTextValue;

        _todoListsViewModel.NewListName = text;
    }
}