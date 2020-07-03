using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Infrastructure.ApiClient
{
    public interface ICalculatePaymentApiClient 
    {
        Task CalculateFirstPayment(long accountId, Guid claimId);
    }
}
