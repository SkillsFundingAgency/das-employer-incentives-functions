using System;
using System.Collections.Generic;
using System.Text;
using Azure.Storage.Blobs;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ManualPayments
{
    public interface IBlobContainerFactory
    {
        BlobContainerClient GetBlobContainerClient(string connectionString, string blobContainerName);
    }
}
