using System.ComponentModel;
using System.Runtime.CompilerServices;
using ZeestMobile.Infrastructure.EntityFramework;
using ZeestMobile.Model;

namespace ZeestMobile.ViewModels;

public class TodoListsViewModel(ApplicationContext applicationContext) : INotifyPropertyChanged
{
    public IReadOnlyList<TodoList> TodoLists => applicationContext.TodoLists.ToList();
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void AddTodoList(string name)
    {
        var todoList = new TodoList(name)
        {
            Items = []
        };

        applicationContext.TodoLists.Add(todoList);
        applicationContext.SaveChanges();
    }
}