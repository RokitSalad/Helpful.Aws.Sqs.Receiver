using System.Threading;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Helpful.Aws.Sqs.Receiver.Sqs;

namespace Helpful.Aws.Sqs.Receiver
{
    public static class ReceiverFactory
    {
        public static IMessageReceiver GetReceiver(string awsAccessKey, string awsSecretKey, string awsRegion, string queueUrl, CancellationToken cancellationToken)
        {
            AWSCredentials creds = new BasicAWSCredentials(awsAccessKey, awsSecretKey);
            var sqsClient = new AmazonSQSClient(creds, RegionEndpoint.GetBySystemName(awsRegion));

            IMessageReceiver receiver = new MessageReceiver(new MessageReceiverConfig
            {
                QueueUrl = queueUrl
            }, new SqsQueueClient(sqsClient, cancellationToken));

            return receiver;
        }
        
        public static IMessageReceiver GetReceiver(string awsAccessKey, string awsSecretKey, string awsRegion, MessageReceiverConfig messageReceiverConfig, CancellationToken cancellationToken)
        {
            AWSCredentials creds = new BasicAWSCredentials(awsAccessKey, awsSecretKey);
            var sqsClient = new AmazonSQSClient(creds, RegionEndpoint.GetBySystemName(awsRegion));

            IMessageReceiver receiver = new MessageReceiver(messageReceiverConfig, new SqsQueueClient(sqsClient, cancellationToken));

            return receiver;
        }

        public static IMessageReceiver GetReceiver(AWSCredentials awsCredentials, string awsRegion, string queueUrl, CancellationToken cancellationToken)
        {
            var sqsClient = new AmazonSQSClient(awsCredentials, RegionEndpoint.GetBySystemName(awsRegion));

            IMessageReceiver receiver = new MessageReceiver(new MessageReceiverConfig
            {
                QueueUrl = queueUrl
            }, new SqsQueueClient(sqsClient, cancellationToken));

            return receiver;
        }

        public static IMessageReceiver GetReceiver(AWSCredentials awsCredentials, string awsRegion, MessageReceiverConfig messageReceiverConfig, CancellationToken cancellationToken)
        {
            var sqsClient = new AmazonSQSClient(awsCredentials, RegionEndpoint.GetBySystemName(awsRegion));

            IMessageReceiver receiver = new MessageReceiver(messageReceiverConfig, new SqsQueueClient(sqsClient, cancellationToken));

            return receiver;
        }

        public static IMessageReceiver GetReceiver(AmazonSQSClient amazonSqsClient, string queueUrl, CancellationToken cancellationToken)
        {
            IMessageReceiver receiver = new MessageReceiver(new MessageReceiverConfig
            {
                QueueUrl = queueUrl
            }, new SqsQueueClient(amazonSqsClient, cancellationToken));

            return receiver;
        }

        public static IMessageReceiver GetReceiver(AmazonSQSClient amazonSqsClient, MessageReceiverConfig messageReceiverConfig, CancellationToken cancellationToken)
        {
            IMessageReceiver receiver = new MessageReceiver(messageReceiverConfig, new SqsQueueClient(amazonSqsClient, cancellationToken));

            return receiver;
        }

        public static IMessageReceiver GetReceiver(IQueueClient amazonQueueClient, string queueUrl)
        {
            IMessageReceiver receiver = new MessageReceiver(new MessageReceiverConfig
            {
                QueueUrl = queueUrl
            }, amazonQueueClient);

            return receiver;
        }

        public static IMessageReceiver GetReceiver(IQueueClient amazonQueueClient, MessageReceiverConfig messageReceiverConfig)
        {
            IMessageReceiver receiver = new MessageReceiver(messageReceiverConfig, amazonQueueClient);

            return receiver;
        }
    }
}