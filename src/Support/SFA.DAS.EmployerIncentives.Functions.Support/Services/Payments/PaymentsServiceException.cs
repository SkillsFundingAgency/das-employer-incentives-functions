using System;
using System.Net;
using System.Runtime.Serialization;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.Payments
{
    [Serializable]
    public class PaymentsServiceException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }
        public string Content { get; }

        public PaymentsServiceException(HttpStatusCode httpStatusCode, string content) : base()
        {
            HttpStatusCode = httpStatusCode;
            Content = content;
        }

        protected PaymentsServiceException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}