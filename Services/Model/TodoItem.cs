namespace Services.Model;

public class TodoItem(string text, bool done, DateTime deadline)
{
    public string Text { get; set; } = text;

    public bool Done { get; set; } = done;

    public DateTime Deadline { get; set; } = deadline;
}
