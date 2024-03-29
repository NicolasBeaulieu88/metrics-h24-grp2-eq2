﻿using MetricsAPI_LOG680.DTO;

namespace MetricsAPI_LOG680.Services
{
    public class PRMetricsService : IPRMetricsService
    {
        public PRLeadTime CreatePRLeadTime(string prNumber, string username, string repository, DateTime savedDate, DateTime createdDate, DateTime closedDate, TimeSpan leadTime)
        {
            var prLeadtime =  new PRLeadTime
            {
                PrId = prNumber,
                Username = username,
                Repository = repository,
                SavedDate = savedDate.ToUniversalTime(),
                CreatedDate = createdDate.ToUniversalTime(),
                ClosedDate = closedDate.ToUniversalTime(),
                LeadTime = leadTime
            };
            return prLeadtime;
        }

        public PRMergedTime CreatePRMergedTime(string prNumber, string username, string repository, DateTime savedDate, DateTime mergedDate, TimeSpan mergedTime)
        {
            var prMergedTime = new PRMergedTime
            {
                PrId = prNumber,
                Username = username,
                Repository = repository,
                SavedDate = savedDate.ToUniversalTime(),
                MergedDate = mergedDate.ToUniversalTime(),
                MergedTime = mergedTime
            };
            return prMergedTime;
        }

        public PRDiscussions CreatePRDiscussions(string prNumber, string username, string repository, DateTime savedDate, int comments, int reviews, int reviewRequests, int totalDiscussions)
        {
            var prDiscussions = new PRDiscussions
            {
                PrId = prNumber,
                Username = username,
                Repository = repository,
                SavedDate = savedDate.ToUniversalTime(),
                Comments = comments,
                Reviews = reviews,
                ReviewRequests = reviewRequests,
                TotalDiscussions = totalDiscussions
            };
            return prDiscussions;
        }

        public PRFlowRatio CreatePRFlowRatio(string username, string repository, DateTime savedDate, int totalItemsClosed, int totalItemsOpened, double flowRatio)
        {
            var prFlowRatio = new PRFlowRatio
            {
                Username = username,
                Repository = repository,
                SavedDate = savedDate.ToUniversalTime(),
                OpenedPR = totalItemsOpened,
                ClosedPR = totalItemsClosed,
                FlowRatio = flowRatio
            };
            return prFlowRatio;
        }

        public PRSize CreatePRSize(string prNumber, string username, string repository, DateTime savedDate, int additions, int deletions, int totalChanges)
        {
            var prSize = new PRSize
            {
                PrId = prNumber,
                Username = username,
                Repository = repository,
                SavedDate = savedDate.ToUniversalTime(),
                Additions = additions,
                Deletions = deletions,
                TotalChanges = totalChanges
            };
            return prSize;
        }

    }
}
