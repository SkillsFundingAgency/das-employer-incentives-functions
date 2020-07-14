using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Infrastructure.ApiClient
{
    public interface ICalculatePaymentApiClient 
    {
        Task<bool> CalculatePayment(long accountId, Guid claimId);
    }
}
