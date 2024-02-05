using MetricsAPI_LOG680.DTO;
using Microsoft.EntityFrameworkCore;

namespace MetricsAPI_LOG680.Repositories;

public class SnapshotRepository : ISnapshotRepository
{
    private readonly ApiDbContext _dbContext;
    
    public SnapshotRepository(ApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<Snapshot>> GetSnapshotsByDates(DateTime startDate, DateTime endDate,
        string? owner, string? repository, string? projectId)
    {
        if (projectId != null)
        {
            return await _dbContext.Snapshots
                .Where(s => s.Project_id == projectId 
                            && s.Timestamps >= startDate.ToUniversalTime()
                            && s.Timestamps <= endDate.ToUniversalTime())
                .ToListAsync();
        }
        if (repository != null && owner != null)
        {
            return await _dbContext.Snapshots
                .Where(s => s.Repository_name == repository && s.Owner == owner
                                                            && s.Timestamps >= startDate.ToUniversalTime()
                                                            && s.Timestamps <= endDate.ToUniversalTime())
                .ToListAsync();
        }
        return await _dbContext.Snapshots
            .Where(s => s.Timestamps >= startDate.ToUniversalTime()
                        && s.Timestamps <= endDate.ToUniversalTime())
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Snapshot>> GetAllSnapshots(string? owner, string? repository, string? projectId)
    {
        if (projectId != null)
        {
            return await _dbContext.Snapshots
                .Where(s => s.Project_id == projectId)
                .ToListAsync();
        }
        if (repository != null && owner != null)
        {
            return await _dbContext.Snapshots
                .Where(s => s.Repository_name == repository && s.Owner == owner)
                .ToListAsync();
        }
        return await _dbContext.Snapshots.ToListAsync();
    }

    public async Task PostSnapshot(Snapshot snapshot)
    {
        await _dbContext.Snapshots.AddAsync(snapshot);
        await _dbContext.SaveChangesAsync();
    }
}