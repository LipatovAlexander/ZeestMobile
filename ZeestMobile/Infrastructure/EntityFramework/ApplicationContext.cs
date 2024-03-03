using Microsoft.EntityFrameworkCore;
using ZeestMobile.Model;

namespace ZeestMobile.Infrastructure.EntityFramework;

public class ApplicationContext(DbContextOptions<ApplicationContext> options)
    : DbContext(options)
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public DbSet<TodoList> TodoLists => Set<TodoList>();
}
