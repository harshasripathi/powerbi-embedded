using System.Net;

namespace powerbi_embedded_api.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException(string message) : base(message)
        {

        }

        public ApiException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; set; }
    }
}