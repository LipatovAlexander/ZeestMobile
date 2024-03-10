using System.ComponentModel;
using System.Runtime.CompilerServices;
using ZeestMobile.Model;

namespace ZeestMobile.ViewModels;

public class TodoListViewModel(TodoList todoList) : INotifyPropertyChanged
{
    private readonly TodoList _todoList = todoList;
    public string Name => _todoList.Name;

    public event PropertyChangedEventHandler? PropertyChanged;

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
}