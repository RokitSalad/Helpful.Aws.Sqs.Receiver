using System.Collections.Generic;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Helpful.Aws.Sqs.Receiver.Sqs
{
    public interface IReceivedMessageBuilder
    {
        IEnumerable<ReceivedMessage> BuildMessages(IEnumerable<Message> responseMessages, IAmazonSQS sqsClient,
            string requestQueueUrl);
    }
}