using MetricsAPI_LOG680.DTO;
using Microsoft.EntityFrameworkCore;

namespace MetricsAPI_LOG680.Repositories;

public class SnapshotJSONRepository : ISnapshotJSONRepository
{
    private readonly ApiDbContext _dbContext;
    
    public SnapshotJSONRepository(ApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<SnapshotJSON>> GetSnapshotsByDates(DateTime startDate, DateTime endDate,
        string? owner, string? repository, string? projectId)
    {
        if (projectId != null)
        {
            return await _dbContext.SnapshotsJSON
                .Where(s => s.Project_id == projectId 
                            && s.Timestamps >= startDate.ToUniversalTime()
                            && s.Timestamps <= endDate.ToUniversalTime())
                .ToListAsync();
        }
        if (repository != null && owner != null)
        {
            return await _dbContext.SnapshotsJSON
                .Where(s => s.Repository_name == repository && s.Owner == owner
                                                            && s.Timestamps >= startDate.ToUniversalTime()
                                                            && s.Timestamps <= endDate.ToUniversalTime())
                .ToListAsync();
        }
        return await _dbContext.SnapshotsJSON
            .Where(s => s.Timestamps >= startDate.ToUniversalTime()
                        && s.Timestamps <= endDate.ToUniversalTime())
            .ToListAsync();
    }
    
    public async Task<IEnumerable<SnapshotJSON>> GetAllSnapshots(string? owner, string? repository, string? projectId)
    {
        if (projectId != null)
        {
            return await _dbContext.SnapshotsJSON
                .Where(s => s.Project_id == projectId)
                .ToListAsync();
        }
        if (repository != null && owner != null)
        {
            return await _dbContext.SnapshotsJSON
                .Where(s => s.Repository_name == repository && s.Owner == owner)
                .ToListAsync();
        }
        return await _dbContext.SnapshotsJSON.ToListAsync();
    }

    public async Task PostSnapshot(SnapshotJSON snapshot)
    {
        await _dbContext.SnapshotsJSON.AddAsync(snapshot);
        await _dbContext.SaveChangesAsync();
    }
}