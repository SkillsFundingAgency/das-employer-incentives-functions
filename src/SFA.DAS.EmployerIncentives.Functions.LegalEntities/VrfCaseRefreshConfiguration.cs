using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class VrfCaseRefreshConfiguration : IVrfCaseRefreshConfiguration
    {
        private static readonly DateTime DefaultLastRunDateTime = new DateTime(2020, 8, 1);
        private readonly CloudTable _table;
        private const string PartitionKey = "LOCAL";
        private const string RowKey = "SFA.DAS.EmployerIncentives.Functions";
        private const string TableName = "RefreshVendorRegistrationFormStatusConfig";

        public VrfCaseRefreshConfiguration(string connectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var tableClient = storageAccount.CreateCloudTableClient();

            _table = tableClient.GetTableReference(TableName);
            _table.CreateIfNotExistsAsync();
        }

        public async Task<DateTime> GetLastRunDateTime()
        {
            var data = await GetEntitiesFromTable<RefreshVendorRegistrationFormStatusData>(_table);

            var record = data?.SingleOrDefault() ?? new RefreshVendorRegistrationFormStatusData
            {
                PartitionKey = PartitionKey,
                RowKey = RowKey,
            };

            return record.LastRunDateTime ?? DefaultLastRunDateTime;
        }

        public async Task UpdateLastRunDateTime(DateTime value)
        {
            var record = new RefreshVendorRegistrationFormStatusData
            {
                PartitionKey = PartitionKey,
                RowKey = RowKey,
                LastRunDateTime = value
            };

            var operation = TableOperation.InsertOrReplace(record);
            await _table.ExecuteAsync(operation);
        }

        private static async Task<IEnumerable<T>> GetEntitiesFromTable<T>(CloudTable table) where T : ITableEntity, new()
        {
            TableQuerySegment<T> querySegment = null;
            var entities = new List<T>();
            var query = new TableQuery<T>();

            do
            {
                querySegment = await table.ExecuteQuerySegmentedAsync(query, querySegment?.ContinuationToken);
                entities.AddRange(querySegment.Results);
            } while (querySegment.ContinuationToken != null);

            return entities;
        }

        private class RefreshVendorRegistrationFormStatusData : TableEntity
        {
            public DateTime? LastRunDateTime { get; set; }
        }

    }
}