namespace ZeestMobile.Model;

public class TodoItem(string text, bool done)
{
    public int Id { get; set; }

    public string Text { get; set; } = text;

    public bool Done { get; set; } = done;
}
