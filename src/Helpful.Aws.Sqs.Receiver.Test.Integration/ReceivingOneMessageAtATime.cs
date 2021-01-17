using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Messages;
using NUnit.Framework;

namespace Helpful.Aws.Sqs.Receiver.Test.Integration
{
    public class ReceivingOneMessageAtATime
    {
        private string _accessKey;
        private string _secretKey;
        private AmazonSQSClient _sqsClient;
        private string _testQueueUrl;

        private readonly string _testReceiveQueueName = $"helpful-aws-sqs-receiver-test-{Guid.NewGuid().ToString().Substring(0,6)}";


        [OneTimeSetUp]
        public async Task Setup()
        {
            _accessKey = Environment.GetEnvironmentVariable("PETE_AUTOMATION_AWS_ACCESS_KEY_ID");
            _secretKey = Environment.GetEnvironmentVariable("PETE_AUTOMATION_AWS_SECRET_KEY");
            AWSCredentials creds = new BasicAWSCredentials(_accessKey, _secretKey);
            _sqsClient = new AmazonSQSClient(creds, RegionEndpoint.APSoutheast2);

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
            //await _sqsClient.DeleteQueueAsync(_testReceiveQueueName);
        }

        [Test]
        public async Task ReceivesMessagesOneAtATimeUsingDefaultSettings()
        {
            IMessageReceiver messageReceiver =
                ReceiverFactory.GetReceiver(_accessKey, _secretKey, "ap-southeast-2", _testQueueUrl, CancellationToken.None);

            List<ReceivedMessage> messages = new List<ReceivedMessage>();

            for (var i = 0; i < 10; i++)
            {
                ReceivedMessage message = await messageReceiver.NextMessageAsync();
                messages.Add(message);
            }

            for (var i = 0; i < 10; i++)
            {
                Assert.That(messages.Select(m => m.OriginalMessageBody).Contains($"message body {i}"));
            }

            Assert.IsNull(await messageReceiver.NextMessageAsync());
        }
    }
}