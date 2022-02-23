using System;
using System.Net;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.BlockPayments
{
    public class BlockPaymentsServiceException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }
        public string Content { get; }

        public BlockPaymentsServiceException(HttpStatusCode httpStatusCode, string content) : base()
        {
            HttpStatusCode = httpStatusCode;
            Content = content;
        }
    }
}
