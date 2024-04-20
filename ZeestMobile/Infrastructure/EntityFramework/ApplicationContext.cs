using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ZeestMobile.Model;

namespace ZeestMobile.Infrastructure.EntityFramework;

public class ApplicationContext(DbContextOptions<ApplicationContext> options)
    : DbContext(options)
{
    public DbSet<TodoList> TodoLists => Set<TodoList>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoList>()
            .HasNoKey()
            .Property(x => x.ToDoItems)
            .HasConversion(
                x => JsonSerializer.Serialize(x, JsonSerializerOptions.Default),
                x =>  JsonSerializer.Deserialize<List<TodoItem>>(x, JsonSerializerOptions.Default)!);
        
        base.OnModelCreating(modelBuilder);
    }
}
