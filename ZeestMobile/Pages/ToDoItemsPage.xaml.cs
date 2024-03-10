using ZeestMobile.ViewModels;

namespace ZeestMobile.Pages;

public partial class ToDoItemsPage : ContentPage
{
    public ToDoItemsPage(TodoListViewModel selectedItem)
    {
        InitializeComponent();
        BindingContext = selectedItem;
    }
}