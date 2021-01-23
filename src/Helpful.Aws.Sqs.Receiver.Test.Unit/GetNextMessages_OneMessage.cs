using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Messages;
using Moq;
using NUnit.Framework;

namespace Helpful.Aws.Sqs.Receiver.Test.Unit
{
    public class GetNextMessages_OneMessage
    {
        private IMessageReceiver _messageReceiver;
        private Mock<IQueueClient> _mockAwsClient;
        private List<ReceivedMessage> _messages;
        private Exception _caughtException;
        private ReceivedMessage _returnedMessage;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _messages = new List<ReceivedMessage>
            {
                new ReceivedMessage()
            };
            _mockAwsClient = new Mock<IQueueClient>();
            _mockAwsClient.Setup(x =>
                    x.GetNextMessagesAsync(It.IsAny<ReceiveMessageRequest>()))
                .ReturnsAsync(_messages);
            _messageReceiver = new MessageReceiver(new MessageReceiverConfig
            {
                QueueUrl = "some url"
            }, _mockAwsClient.Object);

            try
            {
                _returnedMessage = await _messageReceiver.NextMessageAsync();
            }
            catch (Exception e)
            {
                _caughtException = e;
            }
        }

        [Test]
        public void NoExceptionsAreThrown()
        {
            Assert.IsNull(_caughtException);
        }

        [Test]
        public void TheOneReturnedMessageIsReturned()
        {
            Assert.AreEqual(_messages.First(), _returnedMessage);
        }

        [Test]
        public void SHOWAVALUE()
        {
            Assert.AreEqual(1,2, Environment.GetEnvironmentVariable("test-secret"));
        }
    }
}