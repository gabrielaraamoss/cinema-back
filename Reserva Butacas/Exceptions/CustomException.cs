using System;

namespace Reserva_Butacas.Exceptions;

    public class CustomException : Exception
    {
        public CustomException(string message)
            : base(message)
        {
        }

        public CustomException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public string ErrorCode { get; set; }
        public int StatusCode { get; set; } = 500;

        public override string ToString()
        {
            return $"Error Code: {ErrorCode}, Message: {Message}, Inner Exception: {InnerException?.Message}";
        }
    }
