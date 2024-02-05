using MetricsAPI_LOG680.DTO;
using Microsoft.EntityFrameworkCore;

namespace MetricsAPI_LOG680;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<Snapshot> Snapshots { get; set; }
    public DbSet<ColumnActivityCount> ColumnActivityCounts { get; set; }
    public DbSet<FinishedItemsTimeframe> FinishedItemsTimeframes { get; set; }
    public DbSet<LeadTimeTimeframe> LeadTimeTimeframes { get; set; }
    public DbSet<LeadTimePerIssue> LeadTimePerIssues { get; set; }
    public DbSet<PRDiscussions> PRDiscussions { get; set; }
    public DbSet<PRFlowRatio> PRFlowRatios { get; set; }
    public DbSet<PRLeadTime> PRLeadTimes { get; set; }
    public DbSet<PRMergedTime> PRMergedTimes { get; set; }
    public DbSet<PRSize> PRSizes { get; set; }
}