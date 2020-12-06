using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Helpful.Aws.Sqs.Receiver.Messages;
using Moq;
using NUnit.Framework;

namespace Helpful.Aws.Sqs.Receiver.Test.Unit
{
    public class GetNextMessages_NoMessages
    {
        private IMessageReceiver _messageReceiver;
        private Mock<ISqsClient> _mockAwsClient;
        private Exception _caughtException;
        private SqsMessage _returnedMessage;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _mockAwsClient = new Mock<ISqsClient>();
            _mockAwsClient.Setup(x =>
                    x.GetNextMessagesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SqsMessage>());
            _messageReceiver = new MessageReceiver(_mockAwsClient.Object);

            try
            {
                _returnedMessage = await _messageReceiver.NextMessageAsync(CancellationToken.None);
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