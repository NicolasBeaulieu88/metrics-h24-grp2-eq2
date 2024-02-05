using MetricsAPI_LOG680.DTO;
using MetricsAPI_LOG680.Repositories;

namespace MetricsAPI_LOG680.Services
{
    public class SnapshotService : ISnapshotService
    {
        private readonly ISnapshotRepository _repo;
        public SnapshotService(ISnapshotRepository repo)
        {
            _repo = repo;
        }
        public Snapshot CreateSnapshot(int backlogItems, int aFaireItems, int enCoursItems, int revueItems, int termineeItems, DateTime timestamps)
        {
            var totalItems = backlogItems + aFaireItems + enCoursItems + revueItems + termineeItems;
            var snapshot = new Snapshot
            {
                Backlog_items = backlogItems,
                A_faire_items = aFaireItems,
                En_cours_items = enCoursItems,
                Revue_items = revueItems,
                Terminee_items = termineeItems,
                Total_items = totalItems,
                Timestamps = timestamps
            };

            return snapshot;
        }

        public async Task<IEnumerable<Snapshot>> GetSnapshotsByDates(DateTime startDate, DateTime endDate, string? owner, string? repository, string? projectId)
        {
            return await _repo.GetSnapshotsByDates(startDate, endDate, owner, repository, projectId);
        }

        public async Task<IEnumerable<Snapshot>> GetAllSnapshots(string? owner, string? repository, string? projectId)
        {
            return await _repo.GetAllSnapshots(owner, repository, projectId);
        }

        public async Task PostSnapshot(Snapshot snapshot)
        {
            await _repo.PostSnapshot(snapshot);
        }
    }
}
