using ZeestMobile.Model;
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

    private async void CheckBox_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        var checkBox = (CheckBox)sender!;

        var toDoItem = (TodoItem)checkBox.BindingContext;
        toDoItem.Done = e.Value;

        await _todoListViewModel.UpdateAsync(toDoItem);
    }
}