using MetricsAPI_LOG680.DTO;
using Microsoft.EntityFrameworkCore;

namespace MetricsAPI_LOG680;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<Snapshot> Snapshots { get; set; }
}