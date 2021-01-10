using System.Collections.Generic;
using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Exceptions;
using Helpful.Aws.Sqs.Receiver.Messages;

namespace Helpful.Aws.Sqs.Receiver.Sqs
{
    public class ReceiveMessageRequestBuilder : IReceiveMessageRequestBuilder
    {
        private const int MAX_POSSIBLE_WAIT_TIME = 10;
        private const int DEFAULT_VISIBILITY_TIMEOUT = 30;
        private const int FORCE_ONE_MESSAGE_AT_A_TIME = 1;

        public ReceiveMessageRequest Build(MessageReceiverConfig config)
        {
            if (string.IsNullOrWhiteSpace(config.QueueUrl))
            {
                throw new MissingQueueUrlException();
            }
            return new ReceiveMessageRequest
            {
                AttributeNames = config.AttributeNames ?? new List<string> { "All" },
                MaxNumberOfMessages = config.MaxNumberOfMessages ?? FORCE_ONE_MESSAGE_AT_A_TIME,
                QueueUrl = config.QueueUrl,
                VisibilityTimeout = config.VisibilityTimeout ?? DEFAULT_VISIBILITY_TIMEOUT,
                WaitTimeSeconds = config.WaitTimeSeconds ?? MAX_POSSIBLE_WAIT_TIME,
                MessageAttributeNames = config.MessageAttributeNames ?? new List<string> { "All" }
            };
        }
    }
}