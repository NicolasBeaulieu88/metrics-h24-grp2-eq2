using MetricsAPI_LOG680.DTO;

namespace MetricsAPI_LOG680.Services
{
    public class SnapshotService : ISnapshotService
    {
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
    }
}
