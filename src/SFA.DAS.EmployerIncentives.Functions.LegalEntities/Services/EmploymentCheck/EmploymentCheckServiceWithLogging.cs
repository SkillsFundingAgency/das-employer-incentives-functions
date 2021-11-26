using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck.Types;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.EmploymentCheck
{
    public class EmploymentCheckServiceWithLogging : IEmploymentCheckService
    {
        private readonly IEmploymentCheckService _employmentCheckService;
        private readonly ILogger<EmploymentCheckServiceWithLogging> _logger;


        public EmploymentCheckServiceWithLogging(
            IEmploymentCheckService employmentCheckService,
            ILogger<EmploymentCheckServiceWithLogging> logger)
        {
            _employmentCheckService = employmentCheckService;
            _logger = logger;
        }

        public async Task Update(UpdateRequest request)
        {
            try
            {
                _logger.LogInformation("[EmploymentCheck] Calling IEmploymentCheckService.Update with parameters: [correlationId={correlationId}]", request.CorrelationId);

                await _employmentCheckService.Update(request);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[EmploymentCheck] Error calling IEmploymentCheckService.Update with parameters: [correlationId={correlationId}]", request.CorrelationId);

                _logger.LogError(ex, "[EmploymentCheck] Error calling IEmploymentCheckService.Update with parameters: [correlationId={correlationId}]", request.CorrelationId);

                throw;
            }
        }
    }
}
