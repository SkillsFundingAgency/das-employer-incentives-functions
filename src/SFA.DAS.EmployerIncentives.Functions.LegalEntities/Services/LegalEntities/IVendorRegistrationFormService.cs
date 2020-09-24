using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public interface IVendorRegistrationFormService
    {
        Task RefreshVendorRegistrationFormStatuses(DateTime fromDateTime, DateTime toDateTime);
    }
}
