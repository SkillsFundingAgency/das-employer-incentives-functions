using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task BlockAccountLegalEntitiesForPayments(
            List<BlockAccountLegalEntityForPaymentsRequest> blockPaymentsRequest)
        {
            var serviceRequestIdList =
                blockPaymentsRequest.AsEnumerable().Select(x => x.ServiceRequest.TaskId).ToList();
            var taskIds = string.Join(",", serviceRequestIdList);

            try
            {
                _logger.LogInformation(
                    "[BlockPayments] Calling IBlockPaymentsService.BlockAccountLegalEntitiesForPayments for service requests: {TaskIds}",
                    taskIds);

                await _blockPaymentsService.BlockAccountLegalEntitiesForPayments(blockPaymentsRequest);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(
                    "[BlockPayments] Error calling IBlockPaymentsService.BlockAccountLegalEntitiesForPayments for service requests: {TaskIds}",
                    taskIds);

                _logger.LogError(ex,
                    "[BlockPayments] Error calling IBlockPaymentsService.BlockAccountLegalEntitiesForPayments for service requests: {TaskIds}",
                    taskIds);

                throw;
            }
        }
    }
}