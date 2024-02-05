using MetricsAPI_LOG680.DTO;

namespace MetricsAPI_LOG680.Services
{
    public interface IPRMetricsService
    {
        PRLeadTime CreatePRLeadTime(string prNumber, string username, string repository, DateTime savedDate, DateTime createdDate, DateTime closedDate, TimeSpan leadTime);
        PRMergedTime CreatePRMergedTime(string prNumber, string username, string repository, DateTime savedDate, DateTime mergedDate, TimeSpan mergedTime);
        PRDiscussions CreatePRDiscussions(string prNumber, string username, string repository, DateTime savedDate, int comments, int reviews, int reviewRequests, int totalDiscussions);
        PRFlowRatio CreatePRFlowRatio(string username, string repository, DateTime savedDate, int totalItemsClosed, int totalItemsOpened, double flowRatio);
        PRSize CreatePRSize(string prNumber, string username, string repository, DateTime savedDate, int additions, int deletions, int changes);
    }
}
