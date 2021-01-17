using System;
using System.Threading.Tasks;

namespace Helpful.Aws.Sqs.Receiver.Messages
{
    public class ReceivedMessage
    {
        public string OriginalMessageBody { get; set; }

        public Action RemoveFromQueueAsync { get; internal set; }
    }
}