using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Messages;
using Helpful.Aws.Sqs.Receiver.Test.Integration.AWS;
using NUnit.Framework;

namespace Helpful.Aws.Sqs.Receiver.Test.Integration
{
    public class ReceivingMultipleMessagesAtATime
    {
        private AmazonSQSClient _sqsClient;
        private string _testQueueUrl;

        private readonly string _testReceiveQueueName = $"helpful-aws-sqs-receiver-test2-{Guid.NewGuid().ToString().Substring(0,6)}";


        [OneTimeSetUp]
        public async Task Setup()
        {
            _sqsClient = SqsClient.GetAmazonSqsClient();

            await _sqsClient.CreateQueueAsync(new CreateQueueRequest(_testReceiveQueueName));
            GetQueueUrlResponse getQueueUrlResponse = await _sqsClient.GetQueueUrlAsync(_testReceiveQueueName);
            _testQueueUrl = getQueueUrlResponse.QueueUrl;

            for (var i = 0; i < 10; i++)
            {
                await _sqsClient.SendMessageAsync(new SendMessageRequest(_testQueueUrl, $"message body {i}"));
            }
        }

        [OneTimeTearDown]
        public async Task Teardown()
        {
            await _sqsClient.DeleteQueueAsync(_testQueueUrl);
        }

        [Test]
        public async Task ReceivesUpToTenMessagesAtATime()
        {
            IMessageReceiver messageReceiver =
                ReceiverFactory.GetReceiver(AwsKeys.AccessKeyId, AwsKeys.SecretKey, "ap-southeast-2", new MessageReceiverConfig
                {
                    QueueUrl = _testQueueUrl,
                    MaxNumberOfMessages = 10
                }, CancellationToken.None);

            List<ReceivedMessage> messages = new List<ReceivedMessage>();

            for (var i = 0; i < 10; i++)
            {
                ReceivedMessage message = await messageReceiver.NextMessageAsync();
                messages.Add(message);
                message.RemoveFromQueueAsync();
            }

            for (var i = 0; i < 10; i++)
            {
                Assert.That(messages.Select(m => m.OriginalMessageBody).Contains($"message body {i}"));
            }

            Assert.IsNull(await messageReceiver.NextMessageAsync());
        }
    }
}