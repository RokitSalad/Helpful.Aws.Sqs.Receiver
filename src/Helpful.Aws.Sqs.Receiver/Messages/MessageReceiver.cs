using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Exceptions;

namespace Helpful.Aws.Sqs.Receiver.Messages
{
    public class MessageReceiver : IMessageReceiver
    {
        private readonly IQueueClient _queueClient;

        private readonly Queue<ReceivedMessage> _receivedCache;
        private readonly ReceiveMessageRequest _requestConfig;

        public MessageReceiver(MessageReceiverConfig config, IQueueClient queueClient)
        {
            _queueClient = queueClient;
            _requestConfig = BuildReceiveMessageRequest(config);
            _receivedCache = new Queue<ReceivedMessage>();
        }

        public async Task<ReceivedMessage> NextMessageAsync()
        {
            try
            {
                if (!_receivedCache.Any())
                {
                    IEnumerable<ReceivedMessage> messages = await _queueClient.GetNextMessagesAsync(_requestConfig);
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

        private static ReceiveMessageRequest BuildReceiveMessageRequest(MessageReceiverConfig config)
        {
            return new ReceiveMessageRequest
            {
                AttributeNames = config.AttributeNames ?? new List<string> {"All"},
                MaxNumberOfMessages = config.MaxNumberOfMessages,
                QueueUrl = config.QueueUrl,
                VisibilityTimeout = config.VisibilityTimeout,
                WaitTimeSeconds = config.WaitTimeSeconds,
                MessageAttributeNames = config.MessageAttributeNames ?? new List<string> {"All"}
            };
        }
    }

    public class MessageReceiverConfig
    {
        public int MaxNumberOfMessages { get; set; }
        public string QueueUrl { get; set; }
        public int VisibilityTimeout { get; set; }
        public int WaitTimeSeconds { get; set; }
        public List<string> AttributeNames { get; set; }
        public List<string> MessageAttributeNames { get; set; }
    }
}