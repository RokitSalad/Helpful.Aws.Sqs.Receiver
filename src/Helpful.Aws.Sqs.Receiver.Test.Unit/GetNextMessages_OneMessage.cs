using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Helpful.Aws.Sqs.Receiver.Messages;
using Moq;
using NUnit.Framework;

namespace Helpful.Aws.Sqs.Receiver.Test.Unit
{
    public class GetNextMessages_OneMessage
    {
        private IMessageReceiver _messageReceiver;
        private Mock<IQueueClient> _mockAwsClient;
        private List<SqsMessage> _messages;
        private Exception _caughtException;
        private SqsMessage _returnedMessage;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _messages = new List<SqsMessage>
            {
                new SqsMessage()
            };
            _mockAwsClient = new Mock<IQueueClient>();
            _mockAwsClient.Setup(x =>
                    x.GetNextMessagesAsync())
                .ReturnsAsync(_messages);
            _messageReceiver = new MessageReceiver(_mockAwsClient.Object);

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
    }
}