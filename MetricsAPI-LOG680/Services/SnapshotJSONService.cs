﻿using MetricsAPI_LOG680.DTO;
using MetricsAPI_LOG680.Repositories;
using Newtonsoft.Json;

namespace MetricsAPI_LOG680.Services
{
    public class SnapshotJSONService : ISnapshotJSONService
    {
        private readonly ISnapshotJSONRepository _repo;
        public SnapshotJSONService(ISnapshotJSONRepository repo)
        {
            _repo = repo;
        }
        public SnapshotJSON CreateSnapshotJSON(Dictionary<string, int> columnsData, int totalItems, DateTime timestamps)
        {
            var snapshot = new SnapshotJSON
            {
                Columns_data = JsonConvert.SerializeObject(columnsData),
                Total_items = totalItems,
                Timestamps = timestamps
            };

            return snapshot;
        }

        public async Task<IEnumerable<SnapshotJSON>> GetSnapshotsByDates(DateTime startDate, DateTime endDate, string? owner, string? repository, string? projectId)
        {
            return await _repo.GetSnapshotsByDates(startDate, endDate, owner, repository, projectId);
        }

        public async Task<IEnumerable<SnapshotJSON>> GetAllSnapshots(string? owner, string? repository, string? projectId)
        {
            return await _repo.GetAllSnapshots(owner, repository, projectId);
        }

        public async Task PostSnapshot(SnapshotJSON snapshot)
        {
            await _repo.PostSnapshot(snapshot);
        }
    }
}
