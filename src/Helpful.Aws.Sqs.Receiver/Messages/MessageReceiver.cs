using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Helpful.Aws.Sqs.Receiver.Exceptions;

namespace Helpful.Aws.Sqs.Receiver.Messages
{
    public class MessageReceiver : IMessageReceiver
    {
        private readonly IQueueClient _queueClient;

        private readonly Queue<SqsMessage> _receivedCache;

        public MessageReceiver(IQueueClient queueClient)
        {
            _queueClient = queueClient;
            _receivedCache = new Queue<SqsMessage>();
        }

        public async Task<SqsMessage> NextMessageAsync()
        {
            try
            {
                if (!_receivedCache.Any())
                {
                    IEnumerable<SqsMessage> messages = await _queueClient.GetNextMessagesAsync();
                    foreach (var message in messages)
                    {
                        _receivedCache.Enqueue(message);
                    }
                }

                return _receivedCache.Any() ? _receivedCache.Dequeue() : null;
            }
            catch (Exception e)
            {
                throw new SqsMessageReceiveException("Something went wrong while trying to receive a message from SQS.", e);
            }
        }
    }
}