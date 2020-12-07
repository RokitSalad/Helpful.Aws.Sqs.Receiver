using System;

namespace Helpful.Aws.Sqs.Receiver.Exceptions
{
    public class SqsMessageReceiveException : Exception
    {
        public SqsMessageReceiveException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}