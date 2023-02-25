using System;

namespace Reportyy
{
    public class ReportyyApiException : Exception
    {
        public ReportyyApiError Error { get; private set; }

        public ReportyyApiException()
        {
        }

        public ReportyyApiException(string message)
            : base(message)
        {
        }

        public ReportyyApiException(string message, ReportyyApiError error)
            : base(message)
        {
            Error = error;
        }
    }
}

