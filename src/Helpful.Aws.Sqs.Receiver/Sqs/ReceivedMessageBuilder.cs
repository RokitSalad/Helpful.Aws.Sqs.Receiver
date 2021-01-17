using System.Collections.Generic;
using System.Linq;
using Amazon.SQS;
using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Messages;

namespace Helpful.Aws.Sqs.Receiver.Sqs
{
    public class ReceivedMessageBuilder : IReceivedMessageBuilder
    {
        public IEnumerable<ReceivedMessage> BuildMessages(IAmazonSQS sqsClient, string requestQueueUrl,
            IEnumerable<Message> responseMessages)
        {
            if (responseMessages == null)
            {
                return new List<ReceivedMessage>();
            }
            return responseMessages.Select(message => new ReceivedMessage
            {
                OriginalMessageBody = message.Body,
                RemoveFromQueueAsync = () => sqsClient.DeleteMessageAsync(requestQueueUrl, message.ReceiptHandle).Wait()
            }).ToList();
        }
    }
}
