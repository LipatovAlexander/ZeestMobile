using ZeestMobile.ViewModels;

namespace ZeestMobile.Pages;

public partial class ToDoListsPage : ContentPage
{
    private readonly TodoListsViewModel _todoListsViewModel;
    
    public ToDoListsPage(TodoListsViewModel todoListsViewModel)
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

    private async void ListView_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var selectedItem = (TodoListViewModel)e.SelectedItem;
            TodoLists.SelectedItem = null;
            await Navigation.PushAsync(new ToDoItemsPage(selectedItem));
        }
    }
}