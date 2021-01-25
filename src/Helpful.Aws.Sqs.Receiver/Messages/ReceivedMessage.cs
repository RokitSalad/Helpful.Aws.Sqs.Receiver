using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS.Model;

namespace Helpful.Aws.Sqs.Receiver.Messages
{
    public class ReceivedMessage
    {
        public string Body { get; set; }
        public Func<Task<DeleteMessageResponse>> RemoveFromQueueAsync { get; internal set; }
        public string MessageId { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
        public IDictionary<string, MessageAttributeValue> MessageAttributes { get; set; }
    }
}