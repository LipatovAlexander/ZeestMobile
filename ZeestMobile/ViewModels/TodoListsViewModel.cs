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

        Accelerometer.ShakeDetected += OnShakeDetected;
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
        try
        {
            // Установка величины чувствительности датчика при которой считается что было встряхивание
            // Этот параметр можно настроить в зависимости от ваших предпочтений
            Accelerometer.Start(SensorSpeed.UI);
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Ошибка если акселерометр не поддерживается на устройстве
        }
        // Обработка других потенциальных ошибок...
    }

// Метод отписки от акселерометра при необходимости
    private void StopShakeDetection()
    {
        Accelerometer.Stop();
        Accelerometer.ShakeDetected -= OnShakeDetected;
    }

// Обработчик встряхивания
    private void OnShakeDetected(object sender, EventArgs e)
    {
        // Текущий dispatcher вызовет данную функцию в UI потоке
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // Вызов метода для перемешивания списка задач
            ShuffleTodoLists();
        });
    }

// Метод для перемешивания списка задач
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