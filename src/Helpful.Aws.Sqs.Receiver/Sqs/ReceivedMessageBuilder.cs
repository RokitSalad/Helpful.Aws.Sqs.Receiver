using System.Collections.Generic;
using System.Linq;
using Amazon.SQS;
using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Messages;

namespace Helpful.Aws.Sqs.Receiver.Sqs
{
    public class ReceivedMessageBuilder : IReceivedMessageBuilder
    {
        public IEnumerable<ReceivedMessage> BuildMessages(IEnumerable<Message> responseMessages, IAmazonSQS sqsClient,
            string requestQueueUrl)
        {
            if (responseMessages == null)
            {
                return new List<ReceivedMessage>();
            }
            return responseMessages.Select(message => new ReceivedMessage
            {
                Body = message.Body,
                MessageId = message.MessageId,
                Attributes = message.Attributes,
                MessageAttributes = message.MessageAttributes,
                RemoveFromQueueAsync = () => sqsClient.DeleteMessageAsync(requestQueueUrl, message.ReceiptHandle)
            }).ToList();
        }
    }
}
