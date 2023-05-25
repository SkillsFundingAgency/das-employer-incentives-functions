using SFA.DAS.EmployerIncentives.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.Withdrawals.Types
{
    public class WithdrawRequest
    {
        public WithdrawalType WithdrawalType { get; set; }
        public Application[] Applications { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
    }
}
