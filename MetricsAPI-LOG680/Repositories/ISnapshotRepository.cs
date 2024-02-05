using MetricsAPI_LOG680.DTO;

namespace MetricsAPI_LOG680.Repositories;

public interface ISnapshotRepository
{
    Task<IEnumerable<Snapshot>> GetSnapshotsByDates(DateTime startDate, DateTime endDate,
                                                    string? owner, string? repository, string? projectId);

    Task<IEnumerable<Snapshot>> GetAllSnapshots(string? owner, string? repository, string? projectId);

    Task PostSnapshot(Snapshot snapshot);
}