namespace Services.Model;

public class TodoList(string name)
{
    public int Id { get; set; }

    public string Name { get; set; } = name;
    public required List<TodoItem> Items { get; set; }
}