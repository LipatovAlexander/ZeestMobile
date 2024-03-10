using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using ZeestMobile.Infrastructure.EntityFramework;
using ZeestMobile.Model;

namespace ZeestMobile.ViewModels;

public class TodoListsViewModel : INotifyPropertyChanged
{
    private readonly ApplicationContext _applicationContext;

    public ObservableCollection<TodoListViewModel> TodoLists { get; set; }

    public TodoListsViewModel(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
        var lists = _applicationContext.TodoLists
            .Include(tl => tl.Items)
            .ToList()
            .Select(l => new TodoListViewModel(l, applicationContext));
        
        TodoLists = new ObservableCollection<TodoListViewModel>(lists);

        AddItemCommand = new Command(AddTodoList);
    }

    private string _newListName = null!;
    public string NewListName
    {
        get => _newListName;
        set
        {
            if (value != _newListName)
            {
                _newListName = value;
                OnPropertyChanged();                
            }
        }
    }

    public ICommand AddItemCommand { get; }
    
    private void AddTodoList()
    {
        if (string.IsNullOrWhiteSpace(NewListName))
        {
            return;
        }
        
        var todoList = new TodoList(NewListName)
        {
            Items = []
        };

        _applicationContext.TodoLists.Add(todoList);
        _applicationContext.SaveChanges();
        
        TodoLists.Add(new TodoListViewModel(todoList, _applicationContext));
        OnPropertyChanged(nameof(TodoLists));

        NewListName = null!;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}