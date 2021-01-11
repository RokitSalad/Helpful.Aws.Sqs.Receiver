using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Exceptions;
using Helpful.Aws.Sqs.Receiver.Sqs;

namespace Helpful.Aws.Sqs.Receiver.Messages
{
    public class MessageReceiver : IMessageReceiver
    {
        private readonly IQueueClient _queueClient;

        private readonly Queue<ReceivedMessage> _receivedCache;
        private readonly ReceiveMessageRequest _requestConfig;

        public MessageReceiver(MessageReceiverConfig config, IQueueClient queueClient) : 
            this(config, queueClient, new ReceiveMessageRequestBuilder())
        {
        }

        public MessageReceiver(MessageReceiverConfig config,
            IQueueClient queueClient, 
            IReceiveMessageRequestBuilder requestBuilder)
        {
            _queueClient = queueClient;
            _requestConfig = requestBuilder.Build(config);
            _receivedCache = new Queue<ReceivedMessage>();
        }

        public async Task<ReceivedMessage> NextMessageAsync()
        {
            try
            {
                await EnsureCacheAsync();

                return _receivedCache.Any() ? _receivedCache.Dequeue() : null;
            }
            catch (Exception e)
            {
                throw new SqsMessageReceiveException("Something went wrong while trying to receive a message from SQS.", e);
            }
        }

        private async Task EnsureCacheAsync()
        {
            if (!_receivedCache.Any())
            {
                IEnumerable<ReceivedMessage> messages = await _queueClient.GetNextMessagesAsync(_requestConfig);
                foreach (var message in messages)
                {
                    _receivedCache.Enqueue(message);
                }
            }
        }
    }
}