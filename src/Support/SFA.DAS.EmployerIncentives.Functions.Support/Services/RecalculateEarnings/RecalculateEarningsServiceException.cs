using System;
using System.Net;
using System.Runtime.Serialization;

namespace SFA.DAS.EmployerIncentives.Functions.Support.Services.RecalculateEarnings
{
    [Serializable]
    public class RecalculateEarningsServiceException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }
        public string Content { get; }

        public RecalculateEarningsServiceException(HttpStatusCode httpStatusCode, string content) : base()
        {
            HttpStatusCode = httpStatusCode;
            Content = content;
        }

        protected RecalculateEarningsServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
