using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.BlockPayments.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.BlockPayments
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

        public async Task BlockAccountLegalEntitiesForPayments(BlockAccountLegalEntityForPaymentsRequest blockPaymentsRequest)
        {
            try
            {
                _logger.LogInformation("[BlockPayments] Calling IBlockPaymentsService.BlockAccountLegalEntitiesForPayments for service request: {TaskId}", blockPaymentsRequest.ServiceRequest.TaskId);

                await _blockPaymentsService.BlockAccountLegalEntitiesForPayments(blockPaymentsRequest);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[BlockPayments] Error calling IBlockPaymentsService.BlockAccountLegalEntitiesForPayments for service request: {TaskId}", blockPaymentsRequest.ServiceRequest.TaskId);

                _logger.LogError(ex, "[BlockPayments] Error calling IBlockPaymentsService.BlockAccountLegalEntitiesForPayments for service request: {TaskId}", blockPaymentsRequest.ServiceRequest.TaskId);

                throw;
            }
        }
    }
}
