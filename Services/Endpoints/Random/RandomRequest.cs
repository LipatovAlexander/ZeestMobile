namespace Services.Endpoints.Random;

public sealed class RandomRequest
{
    public int? Min { get; set; }
    public int? Max { get; set; }
}