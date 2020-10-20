using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public interface IEmployerVendorIdService
    {
        Task Add(EmployerVendorId employerVendorId);
    }

    public class EmployerVendorId
    {
        public string HashedLegalEntityId { get; set; }
    }

}