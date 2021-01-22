using System;
using System.Threading.Tasks;
using Amazon.SQS.Model;

namespace Helpful.Aws.Sqs.Receiver.Messages
{
    public class ReceivedMessage
    {
        public string OriginalMessageBody { get; set; }

        public Func<Task<DeleteMessageResponse>> RemoveFromQueueAsync { get; internal set; }
    }
}