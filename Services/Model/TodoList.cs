namespace Services.Model;

public class TodoList(string name)
{
    public Guid Id { get; set; }
    public string Name { get; set; } = name;
    public required List<TodoItem> ToDoItems { get; set; }
}