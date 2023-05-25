using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Functions.Support.Services.EmploymentCheck.Types;
using SFA.DAS.EmployerIncentives.Types;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.EmploymentCheck
{
    public class EmploymentCheckValidation : IEmploymentCheckService
    {
        private readonly IEmploymentCheckService _employmentCheckService;

        public EmploymentCheckValidation(IEmploymentCheckService employmentCheckService)
        {
            _employmentCheckService = employmentCheckService;
        }

        public Task Update(UpdateRequest request)
        {
            ValidateUpdateEmploymentChecksRequest(request);

            return _employmentCheckService.Update(request);
        }

        public Task Refresh(IEnumerable<EmploymentCheckRequest> requests)
        {
            if (requests == null || !requests.Any())
            {
                throw new ArgumentException("Request data not set", nameof(requests));
            }

            foreach(var request in requests)
            {
                ValidateRefreshEmploymentChecksRequest(request);
            }

            return _employmentCheckService.Refresh(requests);
        }

        private void ValidateUpdateEmploymentChecksRequest(UpdateRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("Request data not set", nameof(request));
            }

            if (request.CorrelationId == Guid.Empty)
            {
                throw new ArgumentException("Correlation ID not set", nameof(request.CorrelationId));
            }

            if (request.DateChecked == DateTime.MinValue)
            {
                throw new ArgumentException("Date Checked not set", nameof(request.DateChecked));
            }
        }

        private void ValidateRefreshEmploymentChecksRequest(EmploymentCheckRequest request)
        {
            if (String.IsNullOrWhiteSpace(request.CheckType))
            {
                throw new ArgumentException("Employment check type not set", nameof(request.CheckType));
            }

            if (!Enum.TryParse(typeof(RefreshEmploymentCheckType), request.CheckType, false, out var checkType))
            {
                throw new ArgumentException("Invalid employment check type", nameof(request.CheckType));
            }

            if (request.Applications == null || !request.Applications.Any())
            {
                throw new ArgumentException("Applications not set", nameof(request.Applications));
            }

            ValidateServiceRequest(request.ServiceRequest);
        }

        private void ValidateServiceRequest(ServiceRequest serviceRequest)
        {

            if (serviceRequest == null)
            {
                throw new ArgumentException("ServiceRequest not set", nameof(serviceRequest));
            }

            if (string.IsNullOrWhiteSpace(serviceRequest.TaskId))
            {
                throw new ArgumentException("ServiceRequest TaskId not set", nameof(serviceRequest.TaskId));
            }

            if (string.IsNullOrWhiteSpace(serviceRequest.DecisionReference))
            {
                throw new ArgumentException("ServiceRequest DecisionReference not set", nameof(serviceRequest.DecisionReference));
            }

            if (serviceRequest.TaskCreatedDate == null)
            {
                throw new ArgumentException("ServiceRequest TaskCreatedDate not set", nameof(serviceRequest.TaskCreatedDate));
            }
        }
    }
}
