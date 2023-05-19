using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.RecalculateEarnings.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.RecalculateEarnings
{
    public class RecalculateEarningsServiceWithLogging : IRecalculateEarningsService
    {
        private readonly IRecalculateEarningsService _recalculateEarningsService;
        private readonly ILogger<RecalculateEarningsServiceWithLogging> _logger;

        public RecalculateEarningsServiceWithLogging(IRecalculateEarningsService recalculateEarningsService,
            ILogger<RecalculateEarningsServiceWithLogging> logger)
        {
            _recalculateEarningsService = recalculateEarningsService;
            _logger = logger;
        }

        public async Task RecalculateEarnings(RecalculateEarningsRequest recalculateEarningsRequest)
        {
            try
            {
                _logger.LogInformation("[RecalculateEarnings] Calling IRecalculateEarningsService.RecalculateEarnings");
                await _recalculateEarningsService.RecalculateEarnings(recalculateEarningsRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RecalculateEarnings] Error calling IRecalculateEarningsService.RecalculateEarnings");
                throw;
            }
        }
    }
}
