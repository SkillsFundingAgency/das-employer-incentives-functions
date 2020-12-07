using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities.Types;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities
{
    public class ApplicationsService : IApplicationsService
    {
        private readonly HttpClient _client;

        public ApplicationsService(HttpClient client)
        {
            _client = client;
        }

        public Task<IEnumerable<Application>> GetApplications(long accountId)
        {
            throw new NotImplementedException();
        }
    }
}
