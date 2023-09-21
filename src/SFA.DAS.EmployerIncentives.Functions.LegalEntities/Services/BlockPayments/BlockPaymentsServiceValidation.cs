using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.BlockPayments.Types;

#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.BlockPayments
{
    public class BlockPaymentsServiceValidation : IBlockPaymentsService
    {
        private readonly IBlockPaymentsService _blockPaymentsService;

        public BlockPaymentsServiceValidation(IBlockPaymentsService blockPaymentsService)
        {
            _blockPaymentsService = blockPaymentsService;
        }

        public async Task BlockAccountLegalEntitiesForPayments(
            List<BlockAccountLegalEntityForPaymentsRequest> blockPaymentsRequest)
        {
            EnsureRequestIsValid(blockPaymentsRequest);

            await _blockPaymentsService.BlockAccountLegalEntitiesForPayments(blockPaymentsRequest);
        }

        private void EnsureRequestIsValid(List<BlockAccountLegalEntityForPaymentsRequest> requestList)
        {
            foreach (var request in requestList)
            {
                if (request.ServiceRequest == null)
                {
                    throw new ArgumentException("Service Request is not set", nameof(request.ServiceRequest));
                }

                if (string.IsNullOrWhiteSpace(request.ServiceRequest.TaskId))
                {
                    throw new ArgumentException("Service Request Task Id is not set",
                        nameof(request.ServiceRequest.TaskId));
                }

                if (string.IsNullOrWhiteSpace(request.ServiceRequest.DecisionReference))
                {
                    throw new ArgumentException("Service Request Decision Reference is not set",
                        nameof(request.ServiceRequest.DecisionReference));
                }

                if (request.ServiceRequest.TaskCreatedDate == null)
                {
                    throw new ArgumentException("Service Request Task Created Date is not set",
                        nameof(request.ServiceRequest.TaskCreatedDate));
                }

                if (request.VendorBlocks == null || request.VendorBlocks.Count == 0)
                {
                    throw new ArgumentException("Vendor Blocks are not set", nameof(request.VendorBlocks));
                }

                foreach (var vendorBlock in request.VendorBlocks)
                {
                    EnsureVendorBlockIsValid(vendorBlock);
                }
            }
        }

        private void EnsureVendorBlockIsValid(VendorBlock vendorBlock)
        {
            if (string.IsNullOrWhiteSpace(vendorBlock.VendorId))
            {
                throw new ArgumentException("Vendor Block Vendor Id is not set", nameof(vendorBlock.VendorId));
            }

            if (vendorBlock.VendorBlockEndDate == DateTime.MinValue)
            {
                throw new ArgumentException("Vendor Block End Date is not set", nameof(vendorBlock.VendorBlockEndDate));
            }

            if (vendorBlock.VendorBlockEndDate <= DateTime.Now)
            {
                throw new ArgumentException("Vendor Block End Date must be in the future",
                    nameof(vendorBlock.VendorBlockEndDate));
            }
        }
    }
}
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 