using System;
using System.Net;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.RevertPayments
{
    public class RevertPaymentsServiceException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }
        public string Content { get; }

        public RevertPaymentsServiceException(HttpStatusCode httpStatusCode, string content) : base()
        {
            HttpStatusCode = httpStatusCode;
            Content = content;
        }
    }
}