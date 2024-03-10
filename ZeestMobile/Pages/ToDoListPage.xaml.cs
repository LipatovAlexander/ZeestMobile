using ZeestMobile.ViewModels;

namespace ZeestMobile.Pages;

public partial class ToDoListPage : ContentPage
{
    private readonly TodoListViewModel _todoListViewModel;
    public ToDoListPage(TodoListViewModel selectedItem)
    {
        _todoListViewModel = selectedItem;
        InitializeComponent();
        BindingContext = selectedItem;
    }

    private void TodoItem_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var text = e.NewTextValue;

        _todoListViewModel.NewItemName = text;
    }
}