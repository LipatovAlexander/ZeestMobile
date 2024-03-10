using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ZeestMobile.Infrastructure.EntityFramework;
using ZeestMobile.Model;

namespace ZeestMobile.ViewModels;

public class TodoListViewModel : INotifyPropertyChanged
{
    private readonly TodoList _todoList;
    public string Name => _todoList.Name;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ICommand AddItemCommand { get; set; }

    private string _newItemName = null!;
    public string NewItemName
    {
        get => _newItemName;
        set => SetField(ref _newItemName, value);
    }

    public ObservableCollection<TodoItem> TodoItems { get; }

    private readonly ApplicationContext _applicationContext;
    
    public TodoListViewModel(
        TodoList todoList,
        ApplicationContext applicationContext)
    {
        _todoList = todoList;
        AddItemCommand = new Command(AddToDoItem);
        TodoItems = new ObservableCollection<TodoItem>(todoList.Items);
        _applicationContext = applicationContext;
    }

    private void AddToDoItem()
    {
        if (string.IsNullOrWhiteSpace(NewItemName))
        {
            return;
        }

        var item = new TodoItem(NewItemName, false);

        _todoList.Items.Add(item);
        
        _applicationContext.SaveChanges();
        
        TodoItems.Add(item);
        OnPropertyChanged(nameof(TodoItems));

        NewItemName = null!;
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public async Task UpdateAsync(TodoItem toDoItem)
    {
        _applicationContext.TodoItems.Update(toDoItem);
        await _applicationContext.SaveChangesAsync();
    }
}