using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Messages;

namespace Helpful.Aws.Sqs.Receiver.Sqs
{
    public class SqsQueueClient : IQueueClient
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly CancellationToken _cancellationToken;

        public SqsQueueClient(IAmazonSQS sqsClient, CancellationToken cancellationToken)
        {
            _sqsClient = sqsClient;
            _cancellationToken = cancellationToken;
        }

        public async Task<IEnumerable<ReceivedMessage>> GetNextMessagesAsync(ReceiveMessageRequest request)
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                return new List<ReceivedMessage>();
            }

            ReceiveMessageResponse response = await _sqsClient.ReceiveMessageAsync(request, _cancellationToken);
            return BuildMessages(response.Messages);
        }

        private static IEnumerable<ReceivedMessage> BuildMessages(List<Message> responseMessages)
        {
            List<ReceivedMessage> messages = new List<ReceivedMessage>();
            foreach (Message message in responseMessages)
            {
                messages.Add(new ReceivedMessage
                {
                    OriginalMessageBody = message.Body
                });
            }

            return messages;
        }
    }
}
