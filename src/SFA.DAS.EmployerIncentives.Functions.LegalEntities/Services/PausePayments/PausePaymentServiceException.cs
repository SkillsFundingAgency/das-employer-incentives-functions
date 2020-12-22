using System;
using System.Net;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.PausePayments
{
    public class PausePaymentServiceException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }
        public string Content { get; }

        public PausePaymentServiceException(HttpStatusCode httpStatusCode, string content) : base()
        {
            HttpStatusCode = httpStatusCode;
            Content = content;
        }
    }
}