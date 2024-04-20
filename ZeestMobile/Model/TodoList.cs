namespace ZeestMobile.Model;

public class TodoList(string name)
{
    public string Name { get; set; } = name;
    public required List<TodoItem> ToDoItems { get; set; }
}