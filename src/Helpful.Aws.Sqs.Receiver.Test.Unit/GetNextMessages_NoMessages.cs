using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using Helpful.Aws.Sqs.Receiver.Messages;
using Moq;
using NUnit.Framework;

namespace Helpful.Aws.Sqs.Receiver.Test.Unit
{
    public class GetNextMessages_NoMessages
    {
        private IMessageReceiver _messageReceiver;
        private Mock<IQueueClient> _mockAwsClient;
        private Exception _caughtException;
        private ReceivedMessage _returnedMessage;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _mockAwsClient = new Mock<IQueueClient>();
            _mockAwsClient.Setup(x =>
                    x.GetNextMessagesAsync(It.IsAny<ReceiveMessageRequest>()))
                .ReturnsAsync(new List<ReceivedMessage>());
            _messageReceiver = new MessageReceiver(new MessageReceiverConfig(), _mockAwsClient.Object);

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
        public void TheReturnedMessageIsNull()
        {
            Assert.IsNull(_returnedMessage);
        }
    }
}