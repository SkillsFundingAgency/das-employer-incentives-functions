using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.EmploymentCheck.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.EmploymentCheck
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

        public async Task Refresh(IEnumerable<EmploymentCheckRequest> requests)
        {
            _logger.LogInformation($"[EmploymentCheck] Calling IEmploymentCheckService.Refresh for {requests.Count()} service requests");

            await _employmentCheckService.Refresh(requests);
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
