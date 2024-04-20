var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddEndpoints();

var app = builder.Build();

app.MapEndpoints();

app.Run();