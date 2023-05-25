using System;
using System.Net;
using System.Runtime.Serialization;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.BlockPayments
{
    [Serializable]
    public class BlockPaymentsServiceException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }
        public string Content { get; }

        public BlockPaymentsServiceException(HttpStatusCode httpStatusCode, string content) : base()
        {
            HttpStatusCode = httpStatusCode;
            Content = content;
        }

        protected BlockPaymentsServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
