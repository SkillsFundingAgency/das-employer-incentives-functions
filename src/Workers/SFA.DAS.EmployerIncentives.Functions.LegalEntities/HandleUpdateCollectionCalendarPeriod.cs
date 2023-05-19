using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services;
using SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.LegalEntities;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities
{
    public class HandleUpdateCollectionCalendarPeriod
    {
        private readonly ICollectionCalendarService _collectionCalendarService;

        public HandleUpdateCollectionCalendarPeriod(ICollectionCalendarService collectionCalendarService)
        {
            _collectionCalendarService = collectionCalendarService;
        }

        [FunctionName("HttpUpdateCollectionCalendarPeriod")]
        public async Task<IActionResult> RunHttp([HttpTrigger(AuthorizationLevel.Function)] HttpRequest request, ILogger log)
        {
            string validationMessage;
            var updateRequest = CollectionCalendarQueryStringParser.ParseQueryString(request.GetQueryParameterDictionary(), out validationMessage);
            if (updateRequest == null)
            {
                return new BadRequestErrorMessageResult(validationMessage);
            }
            
            await _collectionCalendarService.UpdatePeriod(updateRequest);

            return new OkResult();
        }        
    }
}
