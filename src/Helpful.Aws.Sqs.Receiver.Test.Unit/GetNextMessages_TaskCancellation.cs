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
    public class GetNextMessages_TaskCancellation
    {
        private IMessageReceiver _messageReceiver;
        private Mock<ISqsClient> _mockAwsClient;
        private List<SqsMessage> _messages;
        private Exception _caughtException;
        private SqsMessage _returnedMessage1;
        private SqsMessage _returnedMessage2;
        private SqsMessage _returnedMessage3;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _messages = new List<SqsMessage>
            {
                new SqsMessage(),
                new SqsMessage()
            };
            _mockAwsClient = new Mock<ISqsClient>();
            _mockAwsClient.Setup(x =>
                    x.GetNextMessagesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(_messages);
            _messageReceiver = new MessageReceiver(_mockAwsClient.Object);

            var tokenSource = new CancellationTokenSource();

            try
            {
                _returnedMessage1 = await _messageReceiver.NextMessageAsync(tokenSource.Token);
                tokenSource.Cancel();
                _returnedMessage2 = await _messageReceiver.NextMessageAsync(tokenSource.Token);
                _returnedMessage3 = await _messageReceiver.NextMessageAsync(tokenSource.Token);
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
        public void TheFirstMessageIsReturned()
        {
            Assert.AreEqual(_messages.First(), _returnedMessage1);
        }

        [Test]
        public void TheReceiveCacheIsDrained()
        {
            Assert.AreEqual(_messages.Last(), _returnedMessage2);
        }

        [Test]
        public void NewMessagesAreNotSentAfterCancellation()
        {
            Assert.IsNull(_returnedMessage3);
        }

        [Test]
        public void SqsIsNotCalledAfterCancellation()
        {
            _mockAwsClient.Verify(x => x.GetNextMessagesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}