using Azure.Storage.Blobs;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.ManualPayments
{
    public class BlobContainerFactory : IBlobContainerFactory
    {
        public BlobContainerClient GetBlobContainerClient(string connectionString, string blobContainerName)
        {
            return new BlobContainerClient(connectionString, blobContainerName);
        }
    }
}
