using System.Collections.Generic;
using Amazon.SQS;
using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Messages;

namespace Helpful.Aws.Sqs.Receiver.Sqs
{
    public interface IReceivedMessageBuilder
    {
        IEnumerable<ReceivedMessage> BuildMessages(IAmazonSQS sqsClient, string requestQueueUrl,
            IEnumerable<Message> responseMessages);
    }
}