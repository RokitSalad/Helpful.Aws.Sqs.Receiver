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
        private readonly IReceivedMessageBuilder _receivedMessageBuilder;
        private readonly CancellationToken _cancellationToken;

        public SqsQueueClient(IAmazonSQS sqsClient, CancellationToken cancellationToken) :
            this(sqsClient, new ReceivedMessageBuilder(), cancellationToken)
        {
            
        }

        public SqsQueueClient(IAmazonSQS sqsClient, IReceivedMessageBuilder receivedMessageBuilder, CancellationToken cancellationToken)
        {
            _sqsClient = sqsClient;
            _receivedMessageBuilder = receivedMessageBuilder;
            _cancellationToken = cancellationToken;
        }

        public async Task<IEnumerable<ReceivedMessage>> GetNextMessagesAsync(ReceiveMessageRequest request)
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                return new List<ReceivedMessage>();
            }

            ReceiveMessageResponse response = await _sqsClient.ReceiveMessageAsync(request, _cancellationToken);
            return _receivedMessageBuilder.BuildMessages(response.Messages);
        }
    }
}
