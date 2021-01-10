using System;

namespace Helpful.Aws.Sqs.Receiver.Exceptions
{
    public class MissingQueueUrlException : Exception
    {
        public MissingQueueUrlException() : base("You must provide the URL of the queue from which you want to receive messages.")
        {
            
        }
    }
}