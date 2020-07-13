﻿using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.EmployerIncentives.Functions.AcceptanceTests.Bindings
{
    [Binding]
    public class AzureStorageEmulator
    {
        [BeforeTestRun]
        public static async Task StartAzureStorageEmulator()
        {
            var azureStorageEmulatorExe =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\..\start_azure_storage_emulator.cmd");
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = azureStorageEmulatorExe,
                    Arguments = "start",
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };

            process.Start();

            var storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            var client = storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference("Configuration");
            await table.CreateIfNotExistsAsync();
            await table.ExecuteAsync(TableOperation.InsertOrReplace(new Config("LOCAL", "SFA.DAS.EmployerIncentives.Functions_1.0")));         
        }

        private class Config : ITableEntity
        {
            public string PartitionKey { get; set; }
            public string RowKey { get; set; }
            public DateTimeOffset Timestamp { get; set; }
            public string ETag { get; set; }

            private readonly Dictionary<string, EntityProperty> _data;

            public Config(string partitionKey, string rowKey)
            {
                PartitionKey = partitionKey;
                RowKey = rowKey;
                Timestamp = DateTimeOffset.Now;
                _data = new Dictionary<string, EntityProperty>
                {
                    { "Data", new EntityProperty("{}") }
                };
            }

            public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
            {
                // Method intentionally left empty.
            }

            public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
            {
                return _data;
            }
        }
    }
}