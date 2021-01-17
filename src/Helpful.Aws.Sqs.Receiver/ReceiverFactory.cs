using System.Threading;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Helpful.Aws.Sqs.Receiver.Messages;
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
    }
}