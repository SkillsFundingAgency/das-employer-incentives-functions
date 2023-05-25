using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.Jobs
{
    public interface IJobsService
    {
        Task RefreshLegalEntities(int pageNumber, int pageSize); 
        Task RefreshLearnerMatch();
        Task TriggerPaymentValidation();
        Task TriggerPaymentApproval();
    }
}
