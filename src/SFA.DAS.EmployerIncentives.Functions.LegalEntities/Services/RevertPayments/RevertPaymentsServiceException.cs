using System;
using System.Net;
using System.Runtime.Serialization;

namespace SFA.DAS.EmployerIncentives.Functions.LegalEntities.Services.RevertPayments
{
    [Serializable]
    public class RevertPaymentsServiceException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }
        public string Content { get; }

        public RevertPaymentsServiceException(HttpStatusCode httpStatusCode, string content) : base()
        {
            HttpStatusCode = httpStatusCode;
            Content = content;
        }

        protected RevertPaymentsServiceException(SerializationInfo serializationInfo, StreamingContext streamingContext) 
            : base(serializationInfo, streamingContext)
        {
        }
    }
}