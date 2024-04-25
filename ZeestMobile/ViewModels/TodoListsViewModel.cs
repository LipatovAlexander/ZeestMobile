using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
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
        TodoLists = new ObservableCollection<TodoListViewModel>();

        StartShakeDetection();
        
        Refresh();
        AddItemCommand = new AsyncCommand(AddTodoList, () => true);
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
    
    private async Task AddTodoList()
    {
        if (string.IsNullOrWhiteSpace(NewListName))
        {
            return;
        }
        
        var todoList = new TodoList(NewListName)
        {
            ToDoItems = [],
            Geolocation = await GetGeolocationAsync()
        };

        _applicationContext.TodoLists.Add(todoList);
        await _applicationContext.SaveChangesAsync();
        
        TodoLists.Add(new TodoListViewModel(todoList, _applicationContext));
        OnPropertyChanged(nameof(TodoLists));

        NewListName = null!;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void Refresh()
    {
        var lists = _applicationContext.TodoLists
            .ToList()
            .Select(l => new TodoListViewModel(l, _applicationContext));
        
        TodoLists.Clear();
        foreach (var e in lists)
        {
            TodoLists.Add(e);
        }
    }
    
    private async Task<string> GetGeolocationAsync()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLocationAsync(request);
        
            if (location != null)
            {
                return $"Lat: {location.Latitude}, Lon: {location.Longitude}";
            }
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            return "Геолокация не доступна на этом устройстве";
        }
        catch (PermissionException pEx)
        {
            return "Геолокация не доступна. Нет разрешения";
        }
        catch (Exception ex)
        {
            return $"Геолокация не доступна из-за ошибки {ex.Message}";
        }

        return "Геолокация не доступна";
    }
    
    // Метод для запуска обнаружения встряхиваний
    private void StartShakeDetection()
    {
        if (Accelerometer.Default.IsSupported)
        {
            if (!Accelerometer.Default.IsMonitoring)
            {
                // Turn on accelerometer
                Accelerometer.Default.ShakeDetected += OnShakeDetected;
                Accelerometer.Default.Start(SensorSpeed.UI);
            }
            else
            {
                // Turn off accelerometer
                Accelerometer.Default.Stop();
                Accelerometer.Default.ShakeDetected -= OnShakeDetected;
            }
        }
        else
        {
            Console.WriteLine("Not supported");
        }
    }

    private void OnShakeDetected(object? sender, EventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(ShuffleTodoLists);
    }

    private void ShuffleTodoLists()
    {
        var random = new Random();
        var shuffledLists = TodoLists.OrderBy(x => random.Next()).ToList();

        TodoLists.Clear();
        foreach (var newList in shuffledLists)
        {
            TodoLists.Add(newList);
        }
        OnPropertyChanged(nameof(TodoLists));
    }
}