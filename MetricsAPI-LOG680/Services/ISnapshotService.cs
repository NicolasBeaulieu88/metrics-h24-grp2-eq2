using MetricsAPI_LOG680.DTO;

namespace MetricsAPI_LOG680.Services
{
    public interface ISnapshotService
    {
        Snapshot CreateSnapshot(int backlogItems, int aFaireItems, int enCoursItems, int revueItems, int termineeItems, DateTime timestamps);
        Task<IEnumerable<Snapshot>> GetSnapshotsByDates(DateTime startDate, DateTime endDate,
                                                        string? owner, string? repository, string? projectId);
        Task<IEnumerable<Snapshot>> GetAllSnapshots(string? owner, string? repository, string? projectId);
        Task PostSnapshot(Snapshot snapshot);
    }
}
