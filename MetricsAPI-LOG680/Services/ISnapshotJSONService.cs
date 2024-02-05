using MetricsAPI_LOG680.DTO;

namespace MetricsAPI_LOG680.Services
{
    public interface ISnapshotJSONService
    {
        SnapshotJSON CreateSnapshotJSON(Dictionary<string, int> columnsData, int totalItems, DateTime timestamps);
        Task<IEnumerable<SnapshotJSON>> GetSnapshotsByDates(DateTime startDate, DateTime endDate,
                                                        string? owner, string? repository, string? projectId);
        Task<IEnumerable<SnapshotJSON>> GetAllSnapshots(string? owner, string? repository, string? projectId);
        Task PostSnapshot(SnapshotJSON snapshot);
    }
}
