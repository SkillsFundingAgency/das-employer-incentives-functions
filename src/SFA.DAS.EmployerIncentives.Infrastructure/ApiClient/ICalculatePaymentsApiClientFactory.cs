
namespace SFA.DAS.EmployerIncentives.Infrastructure.ApiClient
{
    public interface ICalculatePaymentsApiClientFactory
    {
        ICalculatePaymentApiClient CreateClient();
    }
}
