using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.BlockPayments.Types;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.BlockPayments
{
    public class BlockPaymentsServiceWithLogging : IBlockPaymentsService
    {

        private readonly IBlockPaymentsService _blockPaymentsService;
        private readonly ILogger<BlockPaymentsServiceWithLogging> _logger;

        public BlockPaymentsServiceWithLogging(
            IBlockPaymentsService blockPaymentsService,
            ILogger<BlockPaymentsServiceWithLogging> logger)
        {
            _blockPaymentsService = blockPaymentsService;
            _logger = logger;
        }

        public async Task BlockAccountLegalEntitiesForPayments(BlockAccountLegalEntityForPaymentsRequest request)
        {
            try
            {
                _logger.LogInformation("[EmploymentCheck] Calling IBlockPaymentsService.BlockAccountLegalEntitiesForPayments for service request: {TaskId}", request.ServiceRequest.TaskId);

                await _blockPaymentsService.BlockAccountLegalEntitiesForPayments(request);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[EmploymentCheck] Error calling IBlockPaymentsService.BlockAccountLegalEntitiesForPayments for service request: {TaskId}", request.ServiceRequest.TaskId);

                _logger.LogError(ex, "[EmploymentCheck] Error calling IBlockPaymentsService.BlockAccountLegalEntitiesForPayments for service request: {TaskId}", request.ServiceRequest.TaskId);

                throw;
            }
        }
    }
}
