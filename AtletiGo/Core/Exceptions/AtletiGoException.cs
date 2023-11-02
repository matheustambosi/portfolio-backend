using System;
using System.Net;

namespace AtletiGo.Core.Exceptions
{
    public class AtletiGoException : Exception
    {
        public HttpStatusCode? HttpStatusCode { get; set; }

        public AtletiGoException(string message) : base(message)
        {
        }

        public AtletiGoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AtletiGoException(string message, HttpStatusCode statusCode) : base(message)
        {
            HttpStatusCode = statusCode;
        }
    }
}
