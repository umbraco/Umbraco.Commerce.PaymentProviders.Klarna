using System;

namespace Umbraco.Commerce.PaymentProviders.Klarna.Api
{
    public class KlarnaApiException : Exception
    {
        public int StatusCode { get; }
        public string ResponseBody { get; }

        public KlarnaApiException(int statusCode, string responseBody, Exception innerException)
            : base($"Klarna API returned {statusCode}: {responseBody}", innerException)
        {
            StatusCode = statusCode;
            ResponseBody = responseBody;
        }
    }
}
