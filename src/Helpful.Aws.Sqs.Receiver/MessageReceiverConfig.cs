using System.Collections.Generic;

namespace Helpful.Aws.Sqs.Receiver
{
    public class MessageReceiverConfig
    {
        public int? MaxNumberOfMessages { get; set; }
        public string QueueUrl { get; set; }
        public int? VisibilityTimeout { get; set; }
        public int? WaitTimeSeconds { get; set; }
        public List<string> AttributeNames { get; set; }
        public List<string> MessageAttributeNames { get; set; }
    }
}